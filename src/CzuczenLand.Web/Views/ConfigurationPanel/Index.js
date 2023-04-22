console.log("crud index.js obecny");

const ConfigurationPanel = {
    EditOrCreate: undefined,
    $dTBox: $("#dataTableSearcher"),
    $s2Box: $("#select2Searcher"),
    $dtDiv: $("#dt-div"),
    $dtCard: $("#dt-card"),
    $dT: undefined,
    $s2: undefined,
    $recordsWithBindActions: [],
    $createBtn: $("#createButton"),

    Init: function () {
        $(document).ready(() => {
            Shared.GameClock();
            this.InitEntitiesSelector();
            this.InitRawTooltip();
            this.BindCreateAction();
            this.BindRecordActions();
            this.BindSelectEntityAction();
            this.BindLogsReader();
            this.BindStructureTests();
            this.BindCleanEditOrCreateModalContent();
            this.BindDistrictCloner();
        });
    },
    InitEntitiesSelector: function () {
        try {
            const select2Options = {
                theme: "classic",
                width: 'resolve',
                language: "pl",
                placeholder: 'Wybierz',
                minimumResultsForSearch: 1,
                multiple: false,
                disabled: false,
            };

            this.$s2 = $(this.$s2Box).select2(select2Options);
            this.$s2.append(this.Helpers.GetOptionsList()).trigger("change.select2");
            this.$s2.val('').trigger('change.select2');
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<")
        }
    },
    InitRawTooltip: function () {
        try {
            const tooltipOptions = {
                template: this.Helpers.GetRawTooltipTemplate(),
                trigger: "manual",
                placement: "auto",
                title: "Brudnopis",
            }

            const $topNavBar = $("#navbar-collapse");
            $topNavBar.tooltip(tooltipOptions);
            $("#draftTextBoxIcon").click(() => $topNavBar.tooltip("toggle"));
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<")
        }
    },
    BindCreateAction: function () {
        const $modal = this.EditOrCreate.$editOrCreateModal;
        this.$createBtn.click(() => {
            abp.ui.setBusy($modal.val());
            try {
                const entityName = this.$createBtn.val();
                abp.ajax({
                    url: abp.appPath + "ConfigurationPanel/EditOrCreateModal?entity=" + entityName,
                    type: 'POST',
                    dataType: 'html',
                    success: (content) => {
                        this.EditOrCreate.$editOrCreateModalContent.html(content);
                        this.EditOrCreate.Init();
                    }
                }).always(() => abp.ui.clearBusy($modal.val()));
            } catch (ex) {
                LogRedError(ex);
                abp.ui.clearBusy($modal.val());
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    BindRecordActions: function () {
        this.$dtDiv.click((event) => {
            try {
                const classNames = event.target.className;
                const entityName = this.$createBtn.val();
                const objectId = $(event.target).closest("tr")?.attr("id")?.slice(4); // usuń row_

                if (event.target.id === this.RecordAction.ActionsNames.EditVisible || event.target.id === this.RecordAction.ActionsNames.EditAll)
                    this.RecordAction.EditMany(event, entityName);
                else if (event.target.id === this.RecordAction.ActionsNames.DeleteVisible || event.target.id === this.RecordAction.ActionsNames.DeleteAll)
                    this.RecordAction.DeleteMany(event, entityName);
                else if (classNames.includes(this.RecordAction.ActionsNames.Delete))
                    this.RecordAction.Delete(event, entityName, objectId);
                else if (classNames.includes(this.RecordAction.ActionsNames.Edit))
                    this.RecordAction.Edit(entityName, objectId);
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<")
            }
        });
    },
    BindSelectEntityAction: function () {
        this.$s2.on("select2:select", (event) => {
            abp.ui.setBusy(this.$dTBox);
            abp.ui.setBusy(this.$dtCard);
            try {
                const entityName = event.params.data.id;
                const hrEntityName = event.params.data.text;

                const $createBtn = this.$createBtn;
                const $actionsBox = $("#actionsBox");
                this.$s2.empty().trigger("change.select2");
                this.$s2.append(this.Helpers.GetOptionsList()).trigger("change.select2");
                this.$s2.val('').trigger('change.select2');

                const _currService = Shared.ServicesLoader(entityName);
                _currService.actionGetAvailableRecords().done((res) => {
                    try {
                        $("#select2-select2Searcher-container").text(hrEntityName);
                        $createBtn.val(entityName);
                        $actionsBox.show();
                        res.canCreate ? $createBtn.show() : $createBtn.hide();

                        this.DataTableHelpers.CreateDataTable(res, entityName);
                        this.Helpers.UpdateRecordsInfo();
                        this.DataTableHelpers.BindStickyDataTableColumns();
                        this.DataTableHelpers.CalculateStickyDataTableColumnsWidth();
                    } catch (ex) {
                        LogRedError(ex);
                        abp.message.error("Coś poszło nie tak :<");
                    }
                }).always(() => {
                    abp.ui.clearBusy(this.$dtCard);
                    abp.ui.clearBusy(this.$dTBox);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.ui.clearBusy(this.$dtCard);
                abp.ui.clearBusy(this.$dTBox);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    BindLogsReader: () => {
        $("#checkLogs").click(() => {
            const $checkLogsModal = $("#checkLogsModal");
            try {
                abp.ui.setBusy($checkLogsModal);
                abp.ajax({
                    url: abp.appPath + "ConfigurationPanel/GetLogs",
                    type: 'get',
                    dataType: 'html',
                    success: (content) => $('#checkLogsModal div.modal-content').html(content)
                }).always(() => abp.ui.clearBusy($checkLogsModal));
            } catch (ex) {
                LogRedError(ex);
                abp.ui.clearBusy($checkLogsModal);
                abp.message.error("Coś poszło nie tak :<")
            }
        });
    },
    BindStructureTests: () => {
        $("#startStructureTests").click(async () => {
            const $structureTestsModal = $("#structureTestsModal");
            try {
                abp.ui.setBusy($structureTestsModal.val());
                abp.ajax({
                    url: abp.appPath + "ConfigurationPanel/StructureTests",
                    type: 'get',
                    dataType: 'html',
                    success: (content) => $('#structureTestsModal div.modal-content').html(content)
                }).always(() => abp.ui.clearBusy($structureTestsModal.val()));
            } catch (ex) {
                LogRedError(ex);
                abp.ui.clearBusy($structureTestsModal.val());
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    BindCleanEditOrCreateModalContent: function () {
        this.EditOrCreate.$editOrCreateModal.on("hide.bs.modal", () => {
            this.EditOrCreate.$editOrCreateModalContent.html("");
        });
    },
    BindDistrictCloner: () => {
        $(".districtClone").click(function () {
            try {
                const controllerUrl = abp.appPath + "District/";
                switch (this.id) {
                    case 'cloneDistrictFromAppFolder':
                        window.open(controllerUrl + "CloneDistrictFromAppFolder", "_blank");
                        break;
                    case 'cloneExistingDistrictToAppFolder':
                        const districtId = $("#districtId").val();
                        window.open(controllerUrl + "CloneExistingDistrictToAppFolderAsCsv/" + districtId, "_blank");
                        break;
                    case 'cloneDistrictFromXlsFiles':
                        const filesIds = $("#filesIds").val();
                        window.open(controllerUrl + "CloneDistrictFromXlsFiles?filesIds=" + filesIds, "_blank");
                        break;
                    case 'cloneDistrictByAnotherExistingDistrict':
                        const districtIdToClone = $("#existingDistrictId").val();
                        const howMany = $("#howMany").val();
                        window.open(controllerUrl + "CloneDistrictByAnotherExistingDistrict/" + districtIdToClone + "?howMany=" + howMany, "_blank");
                        break;
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<")
            }
        });
    },

    RecordAction: {
        ActionsNames: {
            EditVisible: "editVisibleButton",
            EditAll: "editAllButton",
            DeleteVisible: "deleteVisibleButton",
            DeleteAll: "deleteAllButton",
            Edit: "edit-",
            Delete: "delete-"
        },
        EditMany: function (event, entityName) {
            try {
                const $modal = ConfigurationPanel.EditOrCreate.$editOrCreateModal;
                event.stopPropagation();
                const currBtnId = event.target.id;
                const isEditVisible = currBtnId === this.ActionsNames.EditVisible;
                const isEditAll = currBtnId === this.ActionsNames.EditAll;
                const canEditVisible = isEditVisible && ConfigurationPanel.$dT?.rows({page: 'current'}).data().any();
                const canEditAll = isEditAll && ConfigurationPanel.$dT.rows().any();

                if (canEditVisible || canEditAll) {
                    abp.message.confirm(
                        "Edytować " + (currBtnId === this.ActionsNames.EditVisible ? "widoczne rekordy" : "wszystkie rekordy") + "?", "Uwaga!",
                        (confirm1) => {
                            if (confirm1) {
                                abp.message.confirm(
                                    "Jesteś pewien?", "Uwaga!",
                                    (confirm2) => {
                                        if (confirm2) {
                                            const $showHideEditOrCreateModalFakeBtn = $("#showHideEditOrCreateModalFakeBtn");
                                            $showHideEditOrCreateModalFakeBtn.click();
                                            abp.ui.setBusy($modal.val());
                                            try {
                                                abp.ajax({
                                                    url: abp.appPath + "ConfigurationPanel/EditOrCreateModal?entity=" + entityName + "&editManyAction=" + currBtnId,
                                                    type: 'POST',
                                                    dataType: 'html',
                                                    success: (content) => {
                                                        ConfigurationPanel.EditOrCreate.$editOrCreateModalContent.html(content);
                                                        ConfigurationPanel.EditOrCreate.Init();
                                                    }
                                                }).always(() => abp.ui.clearBusy($modal.val()));
                                            } catch (ex) {
                                                LogRedError(ex);
                                                abp.ui.clearBusy($modal.val());
                                                abp.message.error("Coś poszło nie tak :<");
                                            }
                                        }
                                    });
                            }
                        }
                    );
                } else {
                    abp.message.info("Brak rekordów");
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<")
            }
        },
        DeleteMany: function (event, entityName) {
            try {
                const currBtnId = event.target.id;
                const isDeleteVisible = currBtnId === this.ActionsNames.DeleteVisible;
                const isDeleteAll = currBtnId === this.ActionsNames.DeleteAll;
                const canDeleteVisible = isDeleteVisible && ConfigurationPanel.$dT?.rows({page: 'current'}).data().any();
                const canDeleteAll = isDeleteAll && ConfigurationPanel.$dT.rows().any();

                if (canDeleteVisible || canDeleteAll) {
                    abp.message.confirm(
                        "Usunąć " + (currBtnId === this.ActionsNames.DeleteVisible ? "widoczne rekordy" : "wszystkie rekordy") + "?", "Uwaga!",
                        (confirm1) => {
                            if (confirm1) {
                                abp.message.confirm(
                                    "Jesteś pewien?", "Uwaga!",
                                    (confirm2) => {
                                        if (confirm2) {
                                            const ids = currBtnId === this.ActionsNames.DeleteVisible ? ConfigurationPanel.DataTableHelpers.GetVisibleRecordsIds() : []; // puste ids to delete all

                                            abp.ui.setBusy(ConfigurationPanel.$dTBox);
                                            const _currService = Shared.ServicesLoader(entityName);
                                            _currService.actionDeleteMany(ids).done((res) => {
                                                if (res.infoMsg?.length)
                                                    abp.message.info(res.infoMsg);
                                                else {
                                                    let dTIds = Shared.GetDtIds(res.records);
                                                    ConfigurationPanel.$dT.rows(dTIds).remove().draw();
                                                    abp.message.success("", "Usunięto");
                                                }
                                            }).always(() => abp.ui.clearBusy(ConfigurationPanel.$dTBox));
                                        }
                                    });
                            }
                        }
                    );
                } else {
                    event.stopPropagation();
                    abp.message.info("Brak rekordów");
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<")
            }
        },
        Edit: function (entityName, objectId) {
            const $modal = ConfigurationPanel.EditOrCreate.$editOrCreateModal;
            abp.ui.setBusy($modal.val());
            try {
                abp.ajax({
                    url: abp.appPath + "ConfigurationPanel/EditOrCreateModal?entity=" + entityName + "&objectId=" + objectId,
                    type: 'POST',
                    dataType: 'html',
                    success: (content) => {
                        ConfigurationPanel.EditOrCreate.$editOrCreateModalContent.html(content);
                        ConfigurationPanel.EditOrCreate.Init();
                    }
                }).always(() => abp.ui.clearBusy($modal.val()));
            } catch (ex) {
                LogRedError(ex);
                abp.ui.clearBusy($modal.val());
                abp.message.error("Coś poszło nie tak :<");
            }
        },
        Delete: function (event, entityName, objectId) {
            try {
                const recordName = ConfigurationPanel.$dT.row("#row_" + objectId).data()?.Name;
                abp.message.confirm(
                    "Usunąć " + (typeof recordName === "undefined" ? "" : recordName) + "?", "Uwaga!",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            abp.ui.setBusy(ConfigurationPanel.$dTBox);
                            const _currService = Shared.ServicesLoader(entityName);
                            _currService.actionDelete(objectId).done((res) => {
                                try {
                                    if (res.infoMsg?.length)
                                        abp.message.info(res.infoMsg);
                                    else {
                                        const dTIds = Shared.GetDtIds(res.records);
                                        ConfigurationPanel.$dT.rows(dTIds).remove().draw();
                                        abp.message.success("Usunięto");
                                    }
                                } catch (ex) {
                                    LogRedError(ex);
                                    abp.message.error("Coś poszło nie tak :<");
                                }
                            }).always(() => abp.ui.clearBusy(ConfigurationPanel.$dTBox));
                        } else {
                            event.stopPropagation();
                        }
                    }
                );
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<")
            }
        }
    },

    DataTableHelpers: {
        CreateDataTable: function (res, entityName) {
            try {
                const columns = this.GenerateDataTableColumns(res.dbProperties, res.hrProperties, entityName);
                const sortColumnIndex = this.GetSortColumnIndex(columns);

                const dtOpts = this.GetDataTableOptions();
                const opts = Object.assign({}, dtOpts, {
                    columns: columns,
                    order: [[sortColumnIndex, "asc"]]
                });

                ConfigurationPanel.$dT?.destroy(true);
                ConfigurationPanel.$dtDiv.append(ConfigurationPanel.Helpers.GetTable());
                ConfigurationPanel.$dTBox = $("#dataTableSearcher");
                ConfigurationPanel.$dT = ConfigurationPanel.$dTBox.DataTable(opts);

                if (res.records?.length) {
                    const validRes = Shared.MapObjectsFirstCharKeyToUpper(res.records);
                    ConfigurationPanel.$dT.rows.add(validRes).draw();
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        },
        BindStickyDataTableColumns: function () {
            try {
                const $thead = $("thead", ConfigurationPanel.$dTBox);
                const $dataTableThead = $thead.clone(true);
                $dataTableThead.attr("id", "clonedTheadHeads")

                $dataTableThead.insertAfter($thead);
                $dataTableThead.css("display", "none");
                $dataTableThead.css("background-color", "#e9e9e9");
                $dataTableThead.css("z-index", "1");
                $dataTableThead.css("position", "absolute");
                this.BindSortDirectionColumns();
                ConfigurationPanel.$dtDiv.scroll(function () {
                    const $this = $(this);
                    if ($this.scrollTop() > 200) {
                        $dataTableThead.css("top", ($this.scrollTop() - 10) + "px");
                        $dataTableThead.fadeIn(200);
                    } else {
                        $dataTableThead.hide();
                    }
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        },
        GenerateDataTableColumns: function (properties, hrProperties, entityName) {
            const ret = [];
            try {
                let editColumn = {};
                editColumn["data"] = null;
                editColumn["title"] = "Edytuj";
                editColumn["visible"] = true;
                editColumn["searchable"] = false;
                editColumn["orderable"] = false;
                editColumn["defaultContent"] = ConfigurationPanel.Helpers.GetEditAction(entityName);
                editColumn["className"] = "details-control";
                editColumn["width"] = "1%";
                ret.push(editColumn);

                let deleteColumn = {};
                deleteColumn["data"] = null;
                deleteColumn["title"] = "Usuń";
                deleteColumn["visible"] = true;
                deleteColumn["searchable"] = false;
                deleteColumn["orderable"] = false;
                deleteColumn["defaultContent"] = ConfigurationPanel.Helpers.GetDeleteAction(entityName);
                deleteColumn["className"] = "details-control";
                deleteColumn["width"] = "1%";
                ret.push(deleteColumn);

                if (properties.includes("Name")) {
                    let nameColumn = {};
                    nameColumn["data"] = "Name";
                    nameColumn["title"] = "Nazwa";
                    nameColumn["visible"] = true;
                    nameColumn["searchable"] = true;
                    nameColumn["orderable"] = true;
                    nameColumn["render"] = (data, type, row) => {
                        return this.GetTruncateColumnData(type, data);
                    };
                    ret.push(nameColumn);
                }

                if (typeof properties !== "undefined") {
                    for (let i = 0; i < properties?.length; i++) {
                        if (properties[i] !== "Name") {
                            if (properties[i] === "Id") {
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
                                tempObject4["render"] = (data, type, row) => {
                                    return this.GetTruncateColumnData(type, data);
                                };
                                ret.push(tempObject4);
                            }
                        }
                    }
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }

            return ret;
        },
        GetTruncateColumnData: (type, data) => {
            let ret = "";
            try {
                if (type === 'display' && data?.length > 200) {
                    ret = data.substr(0, 200).endsWith(" ") || data.substr(0, 200).endsWith(".")
                        ? data.substr(0, 201) + '…' : data.substr(0, 200) + '…';
                } else {
                    ret = data;
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }

            return ret;
        },
        CalculateStickyDataTableColumnsWidth: () => {
            const $baseThead = $('thead:eq(0) tr:eq(0) th', ConfigurationPanel.$dTBox)
            const $clonedTheadHeads = $("tr:eq(0) th", "#clonedTheadHeads");

            $baseThead.each(function () {
                try {
                    const $this = $(this);
                    $clonedTheadHeads.each(function () {
                        const $clonedThis = $(this);
                        if ($clonedThis.text() === $this.text()) {
                            $clonedThis.css("min-width", $this.css("width"));
                        }
                    });
                } catch (ex) {
                    LogRedError(ex);
                    abp.message.error("Coś poszło nie tak :<")
                }
            });
        },
        GetDataTableOptions: function () {
            return {
                rowId: row => 'row_' + row.Id,
                drawCallback: () => {
                    ConfigurationPanel.Helpers.UpdateRecordsInfo();
                    this.CalculateStickyDataTableColumnsWidth();
                },
                initComplete: () => this.InitColumns(),
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
        InitColumns: () => {
            $('thead tr', ConfigurationPanel.$dTBox).clone().appendTo($("thead", ConfigurationPanel.$dTBox));
            $('thead tr:eq(1) th', ConfigurationPanel.$dTBox).each(function (i) {
                try {
                    const $this = $(this);
                    const title = $this.text();
                    if (title !== "Edytuj" && title !== "Usuń") {
                        title === "Nazwa" ? $this.removeClass("sorting_asc") : null;
                        $this.removeClass("sorting");
                        $this.css("border", "inset");
                        $this.html('<input class="columnSearchInput' + title.replaceAll(" ", "") + '" type="text" placeholder="Szukaj w ' + title + '" />');
                        $('input', $this).on('keyup change', function () {
                            if (ConfigurationPanel.$dT.column(i).search() !== this.value) {
                                ConfigurationPanel.$dT.column(i).search(this.value).draw();
                                const firstThis = this;
                                $(".columnSearchInput" + title.replaceAll(" ", "")).each(function () {
                                    const $this = $(this);
                                    $this.val(firstThis.value)
                                });
                            }
                        });
                    } else {
                        $this.text("");
                        $this.css("border", "ridge");

                        if (title === "Edytuj") {
                            $this.html(ConfigurationPanel.Helpers.GetDropDownMenu(ConfigurationPanel.Helpers.GetEditOptions()))
                        } else {
                            $this.html(ConfigurationPanel.Helpers.GetDropDownMenu(ConfigurationPanel.Helpers.GetDeleteOptions()));
                        }
                    }
                } catch (ex) {
                    LogRedError(ex);
                    abp.message.error("Coś poszło nie tak :<");
                }
            });
        },
        BindSortDirectionColumns: () => {
            const $baseThead = $('thead:eq(0) tr:eq(0) th', ConfigurationPanel.$dTBox)
            const $clonedTheadHeads = $("tr:eq(0) th", "#clonedTheadHeads");

            $clonedTheadHeads.each(function () {
                $(this).click(function () {
                    try {
                        const $currClonedHead = $(this);
                        if ($currClonedHead.hasClass("sorting")) {
                            $currClonedHead.removeClass("sorting");
                            $currClonedHead.addClass("sorting_asc");
                            $clonedTheadHeads.each(function () {
                                const $nextThis = $(this);
                                if ($currClonedHead.text() !== $(this).text() && $(this).text() !== "Edytuj" && $(this).text() !== "Usuń") {
                                    $nextThis.removeClass("sorting_asc");
                                    $nextThis.removeClass("sorting_desc");
                                    $nextThis.addClass("sorting");
                                }
                            });
                        } else if ($currClonedHead.hasClass("sorting_asc")) {
                            $currClonedHead.removeClass("sorting_asc");
                            $currClonedHead.addClass("sorting_desc");
                            $clonedTheadHeads.each(function () {
                                const $nextThis = $(this);
                                if ($currClonedHead.text() !== $(this).text() && $(this).text() !== "Edytuj" && $(this).text() !== "Usuń") {
                                    $nextThis.removeClass("sorting_asc");
                                    $nextThis.removeClass("sorting_desc");
                                    $nextThis.addClass("sorting");
                                }
                            });
                        } else if ($currClonedHead.hasClass("sorting_desc")) {
                            $currClonedHead.removeClass("sorting_desc");
                            $currClonedHead.addClass("sorting_asc");
                            $clonedTheadHeads.each(function () {
                                const $nextThis = $(this);
                                if ($currClonedHead.text() !== $(this).text() && $(this).text() !== "Edytuj" && $(this).text() !== "Usuń") {
                                    $nextThis.removeClass("sorting_asc");
                                    $nextThis.removeClass("sorting_desc");
                                    $nextThis.addClass("sorting");
                                }
                            });
                        }
                    } catch (ex) {
                        LogRedError(ex);
                        abp.message.error("Coś poszło nie tak :<");
                    }
                });
            });

            $baseThead.each(function () {
                $(this).click(function () {
                    const $anotherThis = $(this);
                    $clonedTheadHeads.each(function (i) {
                        try {
                            if ($anotherThis.hasClass("sorting_desc") && $anotherThis.text() === $(this).text()) {
                                $(this).removeClass("sorting");
                                $(this).removeClass("sorting_asc");
                                $(this).addClass("sorting_desc");
                                $clonedTheadHeads.each(function () {
                                    const $nextThis = $(this);
                                    if ($anotherThis.text() !== $(this).text() && $(this).text() !== "Edytuj" && $(this).text() !== "Usuń") {
                                        $nextThis.removeClass("sorting_asc");
                                        $nextThis.removeClass("sorting_desc");
                                        $nextThis.addClass("sorting");
                                    }
                                });
                            } else if ($anotherThis.hasClass("sorting_asc") && $anotherThis.text() === $(this).text()) {
                                $(this).removeClass("sorting");
                                $(this).removeClass("sorting_desc");
                                $(this).addClass("sorting_asc");
                                $clonedTheadHeads.each(function () {
                                    const $nextThis = $(this);
                                    if ($anotherThis.text() !== $(this).text() && $(this).text() !== "Edytuj" && $(this).text() !== "Usuń") {
                                        $nextThis.removeClass("sorting_asc");
                                        $nextThis.removeClass("sorting_desc");
                                        $nextThis.addClass("sorting");
                                    }
                                });
                            }
                        } catch (ex) {
                            LogRedError(ex);
                            abp.message.error("Coś poszło nie tak :<");
                        }
                    });
                });
            });
        },
        GetVisibleRecordsIds: () => {
            try {
                let ids = [];
                const visibleRows = ConfigurationPanel.$dT?.rows({page: 'current'}).data()
                for (let i = 0; i < visibleRows?.length; i++) {
                    const id = visibleRows[i]?.Id;
                    ids.push(id);
                }

                return ids;
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
                return [];
            }
        },
        GetSortColumnIndex: (columns) => {
            try {
                let ret = 2;
                for (let i = 0; i < columns?.length; i++) {
                    if (columns[i]?.data === "Name") {
                        return i;
                    }
                }

                return ret;
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
                return null;
            }
        }
    },

    Helpers: {
        UpdateRecordsInfo: () => {
            try {
                const dTAllRecordsCount = ConfigurationPanel.$dT?.page.info().recordsTotal;
                const dTVisibleRecordsCount = ConfigurationPanel.$dT?.page.info().end;

                const $allRecordsCount = $("#allRecordsCount");
                const $visibleRecordsCount = $("#visibleRecordsCount");

                $allRecordsCount.text(dTAllRecordsCount);
                $visibleRecordsCount.text(dTVisibleRecordsCount);
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        },
        GetOptionsList: () => {
            try {
                const entities = JSON.parse($("#entities").val());

                const optionsList = [];
                for (const entity of entities.DbNames) {
                    optionsList.push(new Option(entities.DbToHrNames[entity], entity, false, false));
                }

                return optionsList;
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
                return [];
            }
        },
        GetDeleteAction: (entityName) => {
            let ret = "";

            ret += '<a id="" class="waves-effect waves-block delete-' + entityName + '">';
            ret += '<i class="material-icons">';
            ret += 'delete';
            ret += '</i>';
            ret += 'Usuń';
            ret += '</a>';

            return ret
        },
        GetEditAction: (entityName) => {
            let ret = "";

            ret += '<a class="waves-effect waves-block edit-' + entityName + '" data-toggle="modal" data-target=".showEditOrCreateModal">';
            ret += '<i class="material-icons">';
            ret += 'edit';
            ret += '</i>';
            ret += 'Edytuj';
            ret += '</a>';

            return ret
        },
        GetTable: () => {
            let ret = "";

            ret += '<table id="dataTableSearcher" class="cell-border hover border-groove">';
            ret += '</table>';

            return ret;
        },
        GetDropDownMenu: (options) => {
            let ret = "";

            ret += '<div class="dropdown">';
            ret += '<a class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">';
            ret += '<i class="material-icons">';
            ret += 'menu';
            ret += '</i>';
            ret += '</a>';
            ret += '<ul class="dropdown-menu pull-left">';
            ret += options;
            ret += '</ul>';
            ret += '</div>';

            return ret;
        },
        GetEditOptions: () => {
            let ret = "";

            ret += '<li>';
            ret += '<a id="editVisibleButton" class="waves-effect waves-block editMany" data-toggle="modal" data-target=".showEditOrCreateModal">';
            ret += '<i class="material-icons">';
            ret += 'colorize';
            ret += '</i>';
            ret += 'Edytuj widoczne';
            ret += '</a>';
            ret += '</li>';
            ret += '<li>';
            ret += '<a id="editAllButton" class="waves-effect waves-block editMany" data-toggle="modal" data-target=".showEditOrCreateModal">';
            ret += '<i class="material-icons">';
            ret += 'border_color';
            ret += '</i>';
            ret += 'Edytuj wszystkie';
            ret += '</a>';
            ret += '</li>';

            return ret;
        },
        GetDeleteOptions: () => {
            let ret = "";

            ret += '<li>';
            ret += '<a id="deleteVisibleButton" class="waves-effect waves-block deleteMany">';
            ret += '<i class="material-icons">';
            ret += 'delete_sweep';
            ret += '</i>';
            ret += 'Usuń widoczne';
            ret += '</a>';
            ret += '</li>';
            ret += '<li>';
            ret += '<a id="deleteAllButton" class="waves-effect waves-block deleteMany">';
            ret += '<i class="material-icons">';
            ret += 'delete_forever';
            ret += '</i>';
            ret += 'Usuń wszystkie';
            ret += '</a>';
            ret += '</li>';

            return ret;
        },
        GetRawTooltipTemplate: () => {
            let ret = "";

            ret += '<div class="m-l--30-percent tooltip draftTooltip">';
            ret += '<div class="tooltip-inner white-space-pre-wrap">';
            ret += '</div>';
            ret += '<textarea>';
            ret += '</textarea>';
            ret += '</div>';

            return ret;
        }
    },
};

ConfigurationPanel.Init();
