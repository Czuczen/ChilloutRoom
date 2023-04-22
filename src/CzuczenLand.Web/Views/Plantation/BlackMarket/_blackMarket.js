console.log("_blackMarket.js obecny");

Plantation.BlackMarket = {
    buyDtSearcher: $("#blackMarketBuyDataTableSearcher"),
    sellDtSearcher: $("#blackMarketSellDataTableSearcher"),
    issuedDtSearcher: $("#blackMarketIssuedDataTableSearcher"),
    buyDt: undefined,
    sellDt: undefined,
    issuedDt: undefined,
    inputsCurrentQuantityData: {},
    inputsCurrentPriceData: {},
    DbActionNames: {
        buy: "buy",
        issue: "issue",
        cancel: "cancel"
    },
    HrActionNames: {
        buy: "Kup",
        issue: "Wystaw",
        cancel: "Anuluj"
    },
    Init: function () {
        $("#showBlackMarket").one("click", async () => {
            const $blackMarketModal = $("#showBlackMarketModal");
            abp.ui.setBusy($blackMarketModal);
            try {
                Plantation.RequestsQueue.addRequest(async () => {
                    await new Promise(resolve => {
                        abp.ajax({
                            url: abp.appPath + "Plantation/GetBlackMarket",
                            type: 'GET',
                            dataType: 'json',
                            success: (res) => {
                                try {
                                    const buyOpts = this.DataTableHelpers.GenerateDataTableOptions(res.buyDbProperties, res.buyHrProperties, this.DbActionNames.buy);
                                    const sellOpts = this.DataTableHelpers.GenerateDataTableOptions(res.sellDbProperties, res.sellHrProperties, this.DbActionNames.issue);
                                    const issuedOpts = this.DataTableHelpers.GenerateDataTableOptions(res.issuedDbProperties, res.issuedHrProperties, this.DbActionNames.cancel);

                                    this.buyDt = this.buyDtSearcher.DataTable(buyOpts);
                                    this.sellDt = this.sellDtSearcher.DataTable(sellOpts);
                                    this.issuedDt = this.issuedDtSearcher.DataTable(issuedOpts);

                                    const validBuyRes = Shared.MapObjectsFirstCharKeyToUpper(res.buyRecords);
                                    this.buyDt.rows.add(validBuyRes).draw();

                                    const validSellRes = Shared.MapObjectsFirstCharKeyToUpper(res.sellRecords);
                                    this.sellDt.rows.add(validSellRes).draw();

                                    const validIssuedRes = Shared.MapObjectsFirstCharKeyToUpper(res.issuedRecords);
                                    this.issuedDt.rows.add(validIssuedRes).draw();

                                    this.BindCancelActions();
                                    this.InitIssueInputs();
                                    this.BindIssueActions();
                                    this.BindBuyActions();
                                } catch (ex) {
                                    LogRedError(ex);
                                    abp.ui.clearBusy($blackMarketModal);
                                    abp.message.error("Coś poszło nie tak :<");
                                }
                            }
                        }).always(() => {
                            abp.ui.clearBusy($blackMarketModal);
                            resolve();
                        });
                    });
                });
            } catch (ex) {
                LogRedError(ex);
                abp.ui.clearBusy($blackMarketModal);
                abp.message.error("Coś poszło nie tak :<");
            }
        });

        this.InitSignalRHandlers();
    },
    InitSignalRHandlers: function () {
        const selectedDistrictId = Plantation.$gameData.data("selected_district");
        const blackMarketHub = Plantation.blackMarketHub;

        blackMarketHub.client.buy = (data) => {
            Plantation.GenerateNotification(data.status, [data.message]);
        };

        blackMarketHub.client.notCanceled = (data) => {
            Plantation.GenerateNotification(data.status, [data.message]);
        };

        blackMarketHub.client.issue = (data) => {
            const districtId = data.districtId;
            if (districtId === selectedDistrictId || districtId === 0)
                Plantation.GenerateNotification(data.status, [data.message]);
        };

        blackMarketHub.client.removeTransaction = (transactionId, itemEntityName) => {
            const dTIds = Shared.GetDtIds([itemEntityName + "-" + transactionId]);
            this.buyDt?.rows(dTIds).remove().draw("full-hold");
        };

        blackMarketHub.client.setTransaction = (transaction) => {
            if (selectedDistrictId === parseInt(transaction.districtId)) {
                const validTransaction = Shared.MapObjectsFirstCharKeyToUpper([transaction]);
                this.buyDt?.rows.add(validTransaction).draw("full-hold");
            }
        };

        blackMarketHub.client.removeIssuedTransaction = (transactionId, itemEntityName) => {
            const dTIds = Shared.GetDtIds([itemEntityName + "-" + transactionId])
            this.issuedDt?.rows(dTIds).remove().draw("full-hold");
        };

        blackMarketHub.client.setIssuedTransaction = (transaction) => {
            if (selectedDistrictId === parseInt(transaction.districtId)) {
                const validTransaction = Shared.MapObjectsFirstCharKeyToUpper([transaction]);
                this.issuedDt?.rows.add(validTransaction).draw("full-hold");
            }
        };
    },
    InitSellBtnsCheckBoxes: () => {
        $(".use-black-market-token").each(function () {
            const id = Shared.MakeId(15);
            const $this = $(this);
            const $label = $this.next("label");

            $this.attr("id", id);
            $label.attr("for", id);
        });
    },
    InitBuyBtnsCheckBoxes: () => {
        $(".use-don-token").each(function () {
            const id = Shared.MakeId(15);
            const $this = $(this);
            const $label = $this.next("label");

            $this.attr("id", id);
            $label.attr("for", id);
        });
    },
    SendIssuedTransaction: function (trId, quantity, price, useBlackMarketToken) {
        const rowData = this.sellDt.row("#" + trId).data();
        const issuedTransaction = {
            ItemId: rowData.ItemId,
            ItemName: rowData.ItemName,
            ItemEntityName: rowData.ItemEntityName,
            Quantity: quantity,
            Price: price
        };
        
        Plantation.RequestsQueue.addRequest(async () => await Plantation.blackMarketHub.server.issue(issuedTransaction, useBlackMarketToken));
    },
    InitSellInputs: function () {
        const $priceInputs = $(".bm-price-input");
        const $quantityInputs = $(".bm-quantity-input");
        const _this = this; 

        $priceInputs.each(function () {
            try {
                const $this = $(this);
                const $tr = $this.closest("tr");
                const trId = $tr.attr("id");
                const rowData = _this.sellDt.row("#" + trId).data();
                const minSellPrice = rowData.BlackMarketMinSellPrice.toFixed(2);
                const maxSellPrice = rowData.BlackMarketMaxSellPrice.toFixed(2);

                $this.attr("min", minSellPrice);
                $this.attr("max", maxSellPrice);

                if (!(trId in _this.inputsCurrentPriceData))
                    $this.val(minSellPrice);
                else if (trId in _this.inputsCurrentPriceData)
                    $this.val(parseFloat(_this.inputsCurrentPriceData[trId].replace(",", ".")));

                $this.prevAll(".current-price").text(parseFloat($this.val()).toFixed(2).replace(".", ","));
                $this.prevAll(".max-price").text(rowData.BlackMarketMaxSellPrice.toFixed(2).replace(".", ","));
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });

        $quantityInputs.each(function () {
            try {
                const $this = $(this);
                const $tr = $this.closest("tr");
                const trId = $tr.attr("id");
                const rowData = _this.sellDt.row("#" + trId).data();
                $this.attr("min", rowData.QuantityInputStep);
                $this.attr("max", rowData.OwnedAmount);
                $this.attr("step", rowData.QuantityInputStep);

                const parsedCurrValue = parseFloat($this.prevAll(".current-quantity").text());
                if (rowData.OwnedAmount === 0) {
                    $this.attr("min", 0);
                    $this.val(0);
                } else if (isNaN(parsedCurrValue) && !(trId in _this.inputsCurrentQuantityData)) {
                    $this.val(rowData.QuantityInputStep);
                } else if (parsedCurrValue > rowData.OwnedAmount) {
                    $this.val(rowData.OwnedAmount);
                } else if (!isNaN(parsedCurrValue) && !(trId in _this.inputsCurrentQuantityData)) {
                    $this.val(parsedCurrValue);
                } else if (trId in _this.inputsCurrentQuantityData) {
                    $this.val(parseFloat(_this.inputsCurrentQuantityData[trId].replace(",", ".")));
                }

                $this.prevAll(".current-quantity").text(parseFloat($this.val()).toString().replace(".", ","));
                $this.prevAll(".item-owned-amount").text(parseFloat(rowData.OwnedAmount).toString().replace(".", ","));
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    BindCancelActions: () => {
        $("#blackMarketIssuedDataTableSearcher").click((event) => {
            try {
                const $target = $(event.target);
                if ($target.hasClass("bm-action-cancel") && !$target.data("clicked")) {
                    $target.data("clicked", true);
                    const $tr = $target.closest("tr");
                    const transactionId = $tr.attr("id").split("-").pop();
                    Plantation.RequestsQueue.addRequest(async () => await Plantation.blackMarketHub.server.cancel(transactionId));
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<")
            }
        });
    },
    InitIssueInputs: function () {
        $("#blackMarketSellDataTableSearcher").on("input", (event) => {
            try {
                const $target = $(event.target);
                const $tr = $target.closest("tr");
                const trId = $tr.attr("id");

                if ($target.hasClass("bm-price-input")) {
                    const currValue = parseFloat($target.val()).toFixed(2).replace(".", ",");
                    $target.prevAll(".current-price").text(currValue);
                    this.inputsCurrentPriceData[trId] = currValue;
                } else if ($target.hasClass("bm-quantity-input")) {
                    const rowData = this.sellDt.row("#" + trId).data();
                    if (rowData.QuantityInputStep.includes(".")) {
                        const currValue = parseFloat($target.val()).toFixed(2).replace(".", ",")
                        $target.prevAll(".current-quantity").text(currValue);
                        this.inputsCurrentQuantityData[trId] = currValue;
                    } else {
                        const currValue = parseFloat($target.val()).toString().replace(".", ",")
                        $target.prevAll(".current-quantity").text(currValue);
                        this.inputsCurrentQuantityData[trId] = currValue;
                    }
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    BindIssueActions: function () {
        $("#blackMarketSellDataTableSearcher").click((event) => {
            try {
                const $target = $(event.target);
                if ($target.hasClass("bm-action-issue")) {
                    const $tr = $target.closest("tr");
                    const trId = $tr.attr("id");
                    const quantity = $("input.bm-quantity-input", "#" + trId).val();

                    if (parseFloat(quantity) === 0)
                        abp.message.info("Wybierz ilość")
                    else {
                        const price = $("input.bm-price-input", "#" + trId).val();
                        const transactionCost = price * quantity;
                        const userId = Plantation.$gameData.data("user_id");
                        const useBlackMarketToken = $target.next("input").prop('checked');

                        if (!useBlackMarketToken) {
                            const donInfo = Plantation.PlantationPanel.DonInfo;
                            if (donInfo?.weHaveDon && donInfo.donId !== userId) {
                                const donTribute = donInfo.donCharityPercentage * transactionCost;
                                abp.message.confirm(
                                    donInfo.donName + " jest don'em dzielnicy. Za wystawienie transakcji musisz mu zapłacić: " +
                                    donTribute.toFixed(2).replace(".", ",") + "$",
                                    "Haracz", (confirmed) => {
                                        if (confirmed)
                                            this.SendIssuedTransaction(trId, quantity, price, useBlackMarketToken);
                                    });
                            } else
                                this.SendIssuedTransaction(trId, quantity, price, useBlackMarketToken);
                        } else
                            this.SendIssuedTransaction(trId, quantity, price, useBlackMarketToken);
                    }
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<")
            }
        });
    },
    BindBuyActions: function () {
        $("#blackMarketBuyDataTableSearcher").click((event) => {
            try {
                const $target = $(event.target);
                if ($target.hasClass("bm-action-buy") && !$target.data("clicked")) {

                    $target.data("clicked", true);
                    const $tr = $target.closest("tr");
                    const transactionId = $tr.attr("id").split("-").pop();
                    const useDonToken = $target.next("input").prop('checked');
                    const donTokensCount = parseInt($("#donToken").text());

                    if (useDonToken) {
                        if (donTokensCount > 0)
                            Plantation.RequestsQueue.addRequest(async () => await Plantation.blackMarketHub.server.buy(transactionId, useDonToken));
                        else {
                            $target.data("clicked", false);
                            Plantation.GenerateNotification("danger", ["Brak żetonów don'a"]);
                        }
                    } else {
                        const rowData = this.buyDt.row("#" + $tr.attr("id")).data();
                        const parsedPrice = parseFloat(rowData.Price.replace(",", ".").replaceAll(" ", ""));
                        const parsedQuantity = parseFloat(rowData.Quantity.replace(",", ".").replaceAll(" ", ""));
                        const cost = parsedPrice * parsedQuantity;
                        const currPlantationGold = parseFloat($("#plantationGold").text().toString().replaceAll(" ", ""));
                        
                        if (currPlantationGold < cost) {
                            $target.data("clicked", false);
                            abp.message.info("Za mało kasy. Koszt: " + cost.toFixed(2).replace(".", ",") + "$");
                        } else {
                            abp.message.confirm("Chcesz kupić " + rowData.Quantity + " " + rowData.ItemName + "? Koszt: " + cost.toFixed(2).replace(".", ",") + "$", " ", (confirm) => {
                                if (confirm) {
                                    Plantation.RequestsQueue.addRequest(async () => {
                                        await Plantation.blackMarketHub.server.buy(transactionId, useDonToken);
                                    });
                                } else {
                                    $target.data("clicked", false);
                                }
                            });
                        }
                    }
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<")
            }
        });
    },
    DataTableHelpers: {
        GenerateDataTableOptions: function (dbProperties, hrProperties, actionName) {
            const columns = this.GenerateDataTableColumns(dbProperties, hrProperties, actionName);
            const sortColumnIndex = this.GetSortColumnIndex(columns);

            const dtOpts = this.GetDataTableOptions();
            return Object.assign({}, dtOpts, {columns: columns, order: [[sortColumnIndex, "asc"]]});
        },
        GetDataTableOptions: () => {
            return {
                rowId: (row) => {
                    const id = typeof row.Id === "undefined" ? row.ItemId : row.Id;
                    return 'row_' + row.ItemEntityName + "-" + id;
                },
                drawCallback: function () {
                    if ($(this).attr("id").includes("Sell")) {
                        Plantation.BlackMarket.InitSellInputs();
                        Plantation.BlackMarket.InitSellBtnsCheckBoxes();
                    }

                    if ($(this).attr("id").includes("Buy"))
                        Plantation.BlackMarket.InitBuyBtnsCheckBoxes();
                },
                retrieve: true,
                bFilter: true,
                fixedHeader: true,
                select: {style: "single"},
                lengthMenu: [5, 10, 25, 50, 75, 100],
                language: {
                    decimal: "",
                    emptyTable: "Brak danych",
                    info: "Pozycje od _START_ do _END_ z _TOTAL_ łącznie",
                    infoEmpty: "Pozycje od 0 do 0 z 0 łącznie",
                    infoFiltered: "(odfiltrowane z _MAX_ wszystkich pozycji)",
                    infoPostFix: ". ",
                    thousands: ", ",
                    lengthMenu: "Pokaż _MENU_ pozycji",
                    loadingRecords: "Szukam...",
                    search: "Szukaj:",
                    zeroRecords: "Brak danych",
                    processing: "Ładowanie...",
                    paginate: {
                        first: "Pierwsza",
                        last: "Ostatnia",
                        next: "Następna",
                        previous: "Poprzednia"
                    },
                    aria: {
                        sortAscending: ": aktywuj, aby posortować kolumny rosnąco",
                        sortDescending: ": aktywuj, aby posortować kolumny malejąco"
                    },
                    select: {
                        rows: {
                            _: "", // Wybrano %d wiersze'y z " + "1" + " dostępnych wyborów
                            0: "", // Kliknij wiersz, aby go wybrać
                            1: "", // Wybrano 1 wiersz z " + "1" + " dostępnych wyborów
                        }
                    }
                },
            }
        },
        GetSortColumnIndex: (columns) => {
            try {
                let ret = 2;
                for (let i = 0; i < columns?.length; i++) {
                    if (columns[i]?.data === "ItemName") {
                        return i;
                    }
                }

                return ret;
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
                return null;
            }
        },
        GenerateDataTableColumns: (properties, hrProperties, actionName) => {
            const ret = [];
            try {
                let actionColumn = {};
                actionColumn["data"] = null;
                actionColumn["title"] = "Akcja";
                actionColumn["visible"] = true;
                actionColumn["searchable"] = false;
                actionColumn["orderable"] = false;
                actionColumn["defaultContent"] = Plantation.BlackMarket.Helpers.GetAction(actionName);
                actionColumn["className"] = "details-control";
                actionColumn["width"] = "1%";
                ret.push(actionColumn);

                if (typeof properties !== "undefined") {
                    for (let i = 0; i < properties?.length; i++) {
                        // TODO: Do unifikacji. Pola specjalnego traktowania
                        if (properties[i] === "Id" ||
                            properties[i] === "ItemId" ||
                            properties[i] === "ItemEntityName" ||
                            properties[i] === "BlackMarketMinSellPrice" ||
                            properties[i] === "BlackMarketMaxSellPrice" ||
                            properties[i] === "OwnedAmount" ||
                            properties[i] === "QuantityInputStep" ||
                            properties[i] === "DistrictId"
                        ) {
                            let tempObject4 = {};
                            tempObject4["data"] = properties[i];
                            tempObject4["title"] = properties[i];
                            tempObject4["visible"] = false;
                            tempObject4["searchable"] = false;
                            ret.push(tempObject4);
                        } else {
                            let tempObject4 = {};
                            tempObject4["data"] = properties[i];
                            tempObject4["title"] = hrProperties[i];
                            tempObject4["visible"] = true;
                            ret.push(tempObject4);
                        }
                    }
                }

                if (actionName === Plantation.BlackMarket.DbActionNames.issue) {
                    let priceColumn = {};
                    priceColumn["data"] = null;
                    priceColumn["title"] = "Cena";
                    priceColumn["visible"] = true;
                    priceColumn["searchable"] = false;
                    priceColumn["orderable"] = false;
                    priceColumn["defaultContent"] = Plantation.BlackMarket.Helpers.GetPriceInput(actionName);
                    ret.push(priceColumn);

                    let quantityColumn = {};
                    quantityColumn["data"] = null;
                    quantityColumn["title"] = "Ilość";
                    quantityColumn["visible"] = true;
                    quantityColumn["searchable"] = false;
                    quantityColumn["orderable"] = false;
                    quantityColumn["defaultContent"] = Plantation.BlackMarket.Helpers.GetQuantityInput(actionName);
                    ret.push(quantityColumn);
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }

            return ret;
        }
    },
    Helpers: {
        GetAction: (actionName) => {
            let ret = "";

            let hrActionName = "";
            for (let property in Plantation.BlackMarket.HrActionNames) {
                if (property === actionName)
                    hrActionName = Plantation.BlackMarket.HrActionNames[property]
            }

            ret += '<div>';
            ret += '<button class="btn btn-info bm-action-' + actionName + '" data-toggle="modal" data-target=".showEditOrCreateModal">';
            ret += hrActionName;
            ret += '</button>';

            if (actionName === Plantation.BlackMarket.DbActionNames.buy) {
                ret += '<input class="use-don-token" type="checkbox"/>';
                ret += '<label title="Użyj żeton dona" class="m-l-5"><span class="material-icons font-size-inherit text-c-grey">radio_button_checked</span></label>';
            }

            if (actionName === Plantation.BlackMarket.DbActionNames.issue) {
                ret += '<input class="use-black-market-token" type="checkbox"/>';
                ret += '<label title="Użyj żeton czarnego rynku" class="m-l-5"><span class="material-icons font-size-inherit text-c-brown">radio_button_checked</span></label>';
            }

            ret += '</div>';

            return ret
        },
        GetPriceInput: () => {
            let ret = "";
            ret += '<span class="current-price"></span>/<span class="max-price"></span><input class="bm-price-input" value="0" type="range" step=0.01>';

            return ret
        },
        GetQuantityInput: () => {
            let ret = "";
            ret += '<span class="current-quantity"></span>/<span class="item-owned-amount"></span><input class="bm-quantity-input" value="0" type="range">';

            return ret
        },
    }
};
