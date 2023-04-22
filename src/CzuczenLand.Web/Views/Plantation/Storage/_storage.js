console.log("_storage.js obecny");

Plantation.Storage = {
    ArtifactActions: {
        Put: "Put",
        Pull: "Pull"    
    },
    ShopActions: {
        Sell: "sell",
        Buy: "buy",    
    },
    Init: function () {
        this.BindRefreshingRangeInputsValue();
        this.BindActiveSellAndBuyBtns();
        this.InitBonuses();
    },
    BindActiveSellAndBuyBtns: function () {
        const $buyBtns = $(".buyBtn");
        const $sellBtns = $(".sellBtn");
        const _this = this;
        
        $buyBtns.click(async function () {
            try {
                const productIdWithEntityName = $(this).attr("id").split("-").pop();
                const $rangeInput = $("#buyAmount-" + productIdWithEntityName);
                const inputValue = $rangeInput.val();
                const entity = $rangeInput.data("entity");
                const productId = productIdWithEntityName.replace(entity, "");

                Plantation.RequestsQueue.addRequest(async () => {
                    const response = await Plantation.storageHub.server.resourcesTransaction(productId, entity, inputValue, _this.ShopActions.Buy);
                    if (response.successfulTransaction)
                        Plantation.GenerateNotification("success", response.infoMessage);
                    else
                        Plantation.GenerateNotification("danger", response.infoMessage);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });

        $sellBtns.click(async function () {
            try {
                const productIdWithEntityName = $(this).attr("id").split("-").pop();
                const $rangeInput = $("#sellAmount-" + productIdWithEntityName);
                const inputValue = $rangeInput.val();
                const entity = $rangeInput.data("entity");
                const productId = productIdWithEntityName.replace(entity, "");
                
                Plantation.RequestsQueue.addRequest(async () => {
                    const response = await Plantation.storageHub.server.resourcesTransaction(productId, entity, inputValue, _this.ShopActions.Sell);

                    if (response.successfulTransaction)
                        Plantation.GenerateNotification("success", response.infoMessage);
                    else
                        Plantation.GenerateNotification("danger", response.infoMessage);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    BindRefreshingRangeInputsValue: () => {
        const $buyAmountInputs = $(".buyAmount");
        const $sellAmountInputs = $(".sellAmount");
        
        $buyAmountInputs.on("input", function () {
            try {
                const $this = $(this);
                const choseAmount = $this.val();
                const productIdWithEntityName = $this.attr("id").split("-").pop();
                $("#buyRangeInputValueBox-" + productIdWithEntityName).text(choseAmount.replaceAll(".", ","));
                const buyPrice = $("#buyPrice-" + productIdWithEntityName).text().toString().replaceAll(" ", "").replace(",", "."); // czyszczenie z przerwy tysięcznej ale backend narazie jej nie dodaje
                const calculateExpense = (parseFloat(choseAmount).toFixed(2) * parseFloat(buyPrice).toFixed(2)).toFixed(2);
                $("#itemsExpense-" + productIdWithEntityName).text(calculateExpense.replaceAll(".", ","));
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });

        $sellAmountInputs.on("input", function () {
            try {
                const $this = $(this);
                const choseAmount = $this.val();
                const productIdWithEntityName = $this.attr("id").split("-").pop();
                $("#sellRangeInputValueBox-" + productIdWithEntityName).text(choseAmount.replaceAll(".", ","));
                const sellPrice = $("#sellPrice-" + productIdWithEntityName).text().replace(",", ".");
                const calculateProfit = (parseFloat(choseAmount).toFixed(2) * parseFloat(sellPrice).toFixed(2)).toFixed(2);
                $("#itemsProfit-" + productIdWithEntityName).text(calculateProfit.replaceAll(".", ","));
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    InitBonuses: function () {
        const $activateBtns = $(".activateBuffBtn");
        const $pullArtifactBtns = $(".pullArtifactBtn");
        const $putArtifactBtns = $(".putArtifactBtn");
        const _this = this;

        $activateBtns.click(async function () {
            try {
                const $this = $(this);
                const productIdWithEntityName = $this.attr("id").split("-").pop();
                const entity = $this.data("entity");
                const bonusId = productIdWithEntityName.replace(entity, "");

                Plantation.RequestsQueue.addRequest(async () => {
                    const response = await Plantation.bonusHub.server.bonusAction(bonusId, "null");
                    if (response.successfulActivation)
                        Plantation.GenerateNotification("success", response.infoMessage);
                    else
                        Plantation.GenerateNotification("danger", response.infoMessage);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
        
        $pullArtifactBtns.click(async function () {
            try {
                const $this = $(this);
                const productIdWithEntityName = $this.attr("id").split("-").pop();
                const entity = $this.data("entity");
                const bonusId = productIdWithEntityName.replace(entity, "");

                Plantation.RequestsQueue.addRequest(async () => {
                    const response = await Plantation.bonusHub.server.bonusAction(bonusId, _this.ArtifactActions.Pull);
                    if (response.successfulActivation) {
                        $this.hide();
                        $("#putArtifact-" + bonusId + entity).show();
                        Plantation.GenerateNotification("success", response.infoMessage);
                    } else
                        Plantation.GenerateNotification("danger", response.infoMessage);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });

        $putArtifactBtns.click(async function () {
            try {
                const $this = $(this);
                const productIdWithEntityName = $this.attr("id").split("-").pop();
                const entity = $this.data("entity");
                const bonusId = productIdWithEntityName.replace(entity, "");

                Plantation.RequestsQueue.addRequest(async () => {
                    const response = await Plantation.bonusHub.server.bonusAction(bonusId, _this.ArtifactActions.Put);
                    if (response.successfulActivation) {
                        $this.hide();
                        $("#pullArtifact-" + bonusId + entity).show();
                        Plantation.GenerateNotification("success", response.infoMessage);
                    } else
                        Plantation.GenerateNotification("danger", response.infoMessage);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
};
