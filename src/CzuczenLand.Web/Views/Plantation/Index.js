console.log("index.js obecny");

const Plantation = {
    Storage: undefined,
    QuestsPanel: undefined,
    PlantationPanel: undefined,
    CustomerZone: undefined,
    BlackMarket: undefined,
    plantHub: $.connection.plantHub,
    infoHub: $.connection.infoHub,
    customersHub: $.connection.customersHub,
    storageHub: $.connection.storageHub,
    questHub: $.connection.questHub,
    blackMarketHub: $.connection.blackMarketHub,
    bonusHub: $.connection.bonusHub,
    $s2DistrictsSelect: undefined,
    $districtSelector: $("#s2DistrictSelector"),
    $gameData: $("#gameData"),
    $plantationContainer: $("#plantation").parent(".container-fluid"),
    creatingPlantData: {},

    Init: function () {
        $(document).ready(() => {
            Shared.GameClock();
            if (Shared.isMobile)
                this.InitScrollArrow();

            this.InitDistrictsSelector();
            this.InitPlantationDistrict();

            if (this.$gameData.data("is_new_player")) return;

            this.PlantationPanel.Init();
            this.Storage.Init();
            this.QuestsPanel.Init();
            this.CustomerZone.Init();
            this.BlackMarket.Init();

            this.InitSignalRErrorHandlers();
            this.InitUpdateStoragesAndProductsStateHandlers();
        });
    },
    InitDistrictsSelector: function () {
        const selectedDistrictId = this.$gameData.data("selected_district");
        const districtsList = this.$gameData.data("districts");
        const districtSelectorOpts = {
            theme: "classic",
            width: 'resolve',
            language: "pl",
            placeholder: "Wybierz dzielnicę",
            minimumResultsForSearch: 1,
            multiple: false,
            disabled: false,
        };

        this.$s2DistrictsSelect = this.$districtSelector.select2(districtSelectorOpts);
        this.$s2DistrictsSelect.append(this.GetDistrictOptionsList(districtsList, selectedDistrictId)).trigger("change.select2");
        this.$s2DistrictsSelect.val('').trigger('change.select2');

        this.$s2DistrictsSelect.on("select2:select", (event) => {
            abp.ui.setBusy(this.$plantationContainer);
            try {
                if (!event.params.data.text.includes("*")) {
                    $("#loadingHollow").show();
                    let createHollowRing = new Audio(CreateHollowRing);
                    createHollowRing.loop = true;
                    createHollowRing.play();
                }

                $("#customersBox").hide(300);
                const districtId = JSON.parse(event.params.data.id);

                this.$s2DistrictsSelect.empty().trigger("change.select2");
                this.$s2DistrictsSelect.append(this.GetDistrictOptionsList(districtsList, districtId)).trigger("change.select2");
                this.$s2DistrictsSelect.val('').trigger('change.select2');

                window.iChangeDistrict = true;
                location.href = abp.appPath + "Plantation/SetDistrict/" + districtId;
            } catch (ex) {
                LogRedError(ex);
                abp.ui.clearBusy(this.$plantationContainer);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    GetDistrictOptionsList: (districtsList, selectedDistrictId) => {
        try {
            const optionsList = [];
            for (const district of districtsList) {
                if (district.Id === selectedDistrictId)
                    optionsList.push(new Option(district.Text, district.Id, false, true));
                else
                    optionsList.push(new Option(district.Text, district.Id, false, false));
            }

            return optionsList;
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
            return [];
        }
    },
    InitPlantationDistrict: function () {
        try {
            const mustBuyHollow = this.$gameData.data("must_buy_hollow");
            const districtLoadingMessage = this.$gameData.data("district_loading_message")?.replaceAll('"', '');
            const isTooLowLevel = this.$gameData.data("too_low_level");

            if (isTooLowLevel)
                return abp.message.info(districtLoadingMessage);

            this.PlantationPanel.InitGuideSwitcher();

            if (mustBuyHollow) {
                abp.message.confirm(districtLoadingMessage, "Siemka", (confirmed) => {
                    if (confirmed) {
                        const selectedDistrictId = this.$gameData.data("selected_district");
                        abp.ui.setBusy(this.$plantationContainer);

                        $("#loadingHollow").show();
                        let createHollowRing = new Audio(CreateHollowRing);
                        createHollowRing.loop = true;
                        createHollowRing.play();

                        window.iChangeDistrict = true;
                        location.href = abp.appPath + "Plantation/SetDistrict/" + selectedDistrictId + "?heWantPayForHollow=" + true;
                    }
                });
            } else if (districtLoadingMessage !== undefined)
                this.GenerateNotification("danger", [districtLoadingMessage]);
        } catch (ex) {
            LogRedError(ex);
            abp.ui.clearBusy(this.$plantationContainer);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    InitSignalRErrorHandlers: function () {
        this.plantHub.client.errorOccured = (ex) => {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :((");
        };

        this.infoHub.client.errorOccured = (ex) => {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :((");
        };

        this.customersHub.client.errorOccured = (ex) => {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :((");
        };

        this.storageHub.client.errorOccured = (ex) => {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :((");
        };

        this.questHub.client.errorOccured = (ex) => {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :((");
        };

        this.blackMarketHub.client.errorOccured = (ex) => {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :((");
        };

        this.bonusHub.client.errorOccured = (ex) => {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :((");
        };
    },
    InitUpdateStoragesAndProductsStateHandlers: function () {
        const infoHub = this.infoHub;
        infoHub.client.updateDon = (donData) => this.PlantationPanel.SetDonContext(donData);

        infoHub.client.changeInfo = (infoData) => { // informacja o aktualizacji dzielnicy przez opiekuna lub administratora
            const selectedDistrictId = Plantation.$gameData.data("selected_district");
            if (infoData.districtId === 0 || selectedDistrictId === infoData.districtId) // jeśli districtId === 0 to administrator dokonał zmiany  
                abp.message.confirm(infoData.infoMessage, "Uwaga!", () => location.reload(true));
        };
        
        infoHub.client.districtChanged = (districtId) => {
            // Jeśli to w tej przeglądarce zmieniono dzielnicę nic nie robimy bo strona już się ładuje. Inaczej nieskończona pętla ładowania strony.
            if (window.iChangeDistrict) return;

            this.$s2DistrictsSelect?.children()?.each(function () {
                if (parseInt(this.value) === districtId)
                    location.href = abp.appPath + "Plantation";
            });
        };

        infoHub.client.updateProduct = (res) => {
            try {
                const rowId = "#row_" + res.entityName + "-" + res.id;
                const rowData = this.BlackMarket.sellDt?.row(rowId).data();
                if (typeof rowData?.OwnedAmount !== "undefined") {
                    rowData.OwnedAmount = res.ownedAmount;
                    const validData = Shared.MapObjectsFirstCharKeyToUpper([rowData]);

                    this.BlackMarket.sellDt.rows(rowId).remove();
                    this.BlackMarket.sellDt.rows.add(validData).draw("full-hold");
                }

                for (let i = 0; i < Object.keys(this.creatingPlantData).length; i++) {
                    const entityName = Object.keys(this.creatingPlantData)[i];
                    const productId = this.creatingPlantData[entityName];
                    if (entityName === res.entityName && productId === res.id)
                        $(".select2-selection__placeholder", "#select2-" + entityName + "-container").text(res.recordName + " Ilość: " + res.ownedAmountWithMeasureUnit);
                }

                $("#ownedAmount-" + res.id + res.entityName).text(res.ownedAmountWithMeasureUnit);
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };

        infoHub.client.updatePlantationState = (res) => {
            try {
                const $plantationLevel = $("#plantationLevel");
                const $plantationCurrExpBar = $("#plantationCurrExpBar");
                const $plantationCurrExp = $("#plantationCurrExp");
                const $plantationExpToNextLevel = $("#plantationExpToNextLevel");
                const $plantationGainedExp = $("#plantationGainedExp");
                const $plantationGold = $("#plantationGold");
                const $storageGold = $("#storageGold");

                const $donToken = $("#donToken");
                const $unlockToken = $("#unlockToken");
                const $questToken = $("#questToken");
                const $dealerToken = $("#dealerToken");
                const $blackMarketToken = $("#blackMarketToken");
                const $prestige = $("#prestige");

                const $goldCurrency = $("#ownedAmount-" + res.plantationStorageId + "Gold");
                const $unlockTokenCurrency = $("#ownedAmount-" + res.plantationStorageId + "UnlockToken");
                const $donTokenCurrency = $("#ownedAmount-" + res.plantationStorageId + "DonToken");
                const $questTokenCurrency = $("#ownedAmount-" + res.plantationStorageId + "QuestToken");
                const $dealerTokenCurrency = $("#ownedAmount-" + res.plantationStorageId + "DealerToken");
                const $blackMarketTokenCurrency = $("#ownedAmount-" + res.plantationStorageId + "BlackMarketToken");
                const $prestigeCurrency = $("#ownedAmount-" + res.plantationStorageId + "Prestige");

                const $weeklyQuestsInProgressCount = $("#weeklyQuestsInProgressCount");
                const $dailyQuestsInProgressCount = $("#dailyQuestsInProgressCount");
                const $artifactsPlaces = $("#artifactsPlaces");
                const $buffsPlaces = $("#buffsPlaces");
                const $weeklyQuestsPlaces = $("#weeklyQuestsPlaces");
                const $dailyQuestsPlaces = $("#dailyQuestsPlaces");
                const $addMaxBuffSlots = $("#addMaxBuffSlots");
                const $addMaxArtifactsSlots = $("#addMaxArtifactsSlots");

                const expPercent = (parseFloat(res.currExp) / parseFloat(res.expToNextLevel)) * 100;

                $plantationLevel.text(res.level);
                $plantationCurrExpBar.css("width", expPercent + "%");
                $plantationCurrExpBar.attr('aria-valuenow', expPercent);
                $plantationCurrExp.text(res.parsedCurrExp + "pkt");
                $plantationExpToNextLevel.text(res.parsedExpToNextLevel + "pkt");
                $plantationGainedExp.text("Zdobyte doświadczenie: " + res.gainedExperience + "pkt");
                $storageGold.text(res.gold);
                $plantationGold.text(res.gold);
                
                if (parseInt($donToken.text()) < res.donToken ||
                    parseInt($unlockToken.text()) < res.unlockToken ||
                    parseInt($questToken.text()) < res.questToken ||
                    parseInt($dealerToken.text()) < res.dealerToken ||
                    parseInt($blackMarketToken.text()) < res.blackMarketToken) {
                    let levelRing = new Audio(TokenRing);
                    levelRing.play();   
                }
                
                $donToken.text(res.donToken);
                $unlockToken.text(res.unlockToken);
                $questToken.text(res.questToken);
                $dealerToken.text(res.dealerToken);
                $blackMarketToken.text(res.blackMarketToken);
                $prestige.text(res.prestige);
                
                $goldCurrency.text(res.gold + "$");
                $unlockTokenCurrency.text(res.unlockToken + "szt.");
                $donTokenCurrency.text(res.donToken + "szt.");
                $questTokenCurrency.text(res.questToken + "szt.");
                $dealerTokenCurrency.text(res.dealerToken + "szt.");
                $blackMarketTokenCurrency.text(res.blackMarketToken + "szt.");
                $prestigeCurrency.text(res.prestige + "pkt");

                $dailyQuestsInProgressCount.text(res.dailyQuestsInProgressCount + "/" + res.unlockedDailyQuestsCount);
                $weeklyQuestsInProgressCount.text(res.weeklyQuestsInProgressCount + "/" + res.unlockedWeeklyQuestsCount);

                $weeklyQuestsPlaces.text("Miejsca zadań tygodniowych " + res.weeklyQuestsPlaces);
                $dailyQuestsPlaces.text("Miejsca zadań dziennych " + res.dailyQuestsPlaces);
                $artifactsPlaces.text("Miejsca na artefakty " + res.artifactsPlaces);
                $buffsPlaces.text("Miejsca na wzmocnienia " + res.buffsPlaces);

                if (res.unlockBuffBtnVisible)
                    $addMaxBuffSlots.show();
                else
                    $addMaxBuffSlots.hide();

                if (res.unlockArtifactBtnVisible)
                    $addMaxArtifactsSlots.show();
                else
                    $addMaxArtifactsSlots.hide();

                this.LevelUpAction(res.receivedLevels);
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };

        infoHub.client.updatePlayerState = (res) => {
            try {
                const $playerLevel = $("#playerLevel");
                const $playerGainedExp = $("#playerGainedExp");
                const $playerGold = $("#playerGold");
                const $honor = $("#honor");

                $honor.text(res.honor);
                $playerLevel.text(res.level);
                $playerGainedExp.data("gained_experience", res.gainedExperience);
                $playerGainedExp.text("Zdobyte doświadczenie: " + res.gainedExperience + "pkt");
                $playerGold.text(res.gold)
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };

        infoHub.client.itemsForNewLvls = (data) => {
            try {
                this.GenerateNotification("info", data.itemsNames);
                for (let i = 0; i < data.questsData?.length; i++) {
                    const questId = data.questsData[i]?.questId;
                    const questType = data.questsData[i]?.questType;
                    this.QuestsPanel.QuestAction.GetAvailableQuestAction(questId, questType);
                }

                if (data.s2DistrictsList?.length) {
                    const selectedDistrictId = this.$gameData.data("selected_district");
                    this.$s2DistrictsSelect.empty().trigger("change.select2");
                    this.$s2DistrictsSelect.append(this.GetDistrictOptionsList(Shared.MapObjectsFirstCharKeyToUpper(data.s2DistrictsList), selectedDistrictId)).trigger("change.select2");
                    this.$s2DistrictsSelect.val('').trigger('change.select2');
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };
    },
    LevelUpAction: (receivedLevels) => {
        try {
            const levels = parseInt(receivedLevels);
            if (levels > 0) {
                let levelRing = new Audio(LevelUpRing);
                levelRing.play();

                const $playerLevel = $("#playerLevel");
                const $plantationLevel = $("#plantationLevel");

                $playerLevel.addClass("blink_me");
                $plantationLevel.addClass("blink_me");

                Shared.WaitSomeMs(5000).then(() => $playerLevel.removeClass("blink_me"));
                Shared.WaitSomeMs(5000).then(() => $plantationLevel.removeClass("blink_me"));
            }
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    InitScrollArrow: () => {
        $(".arrow-scroll-up").click(() => window.scrollTo({top: 0, behavior: 'smooth'}));

        $(window).scroll(function () {
            if ($(this).scrollTop() > 300)
                $(".arrow-scroll-up").fadeIn();
            else
                $(".arrow-scroll-up").fadeOut();
        });
    },
    GenerateNotification: (status, messagesArray, isGuideNotification) => {
        try {
            const $notifications = $("#notificationsBox > div > div.alert:not(.template)");
            if ($notifications.length > 4)
                $notifications[0]?.remove();

            const $notificationTemplate = $($("#notificationsBox > div > div.template.alert-" + status).clone());

            for (let i = 0; i < messagesArray.length; i++) {
                const messageSpan = "<span class='d-block'>" + messagesArray[i] + "</span>";
                $notificationTemplate.append(messageSpan);
            }

            if (isGuideNotification)
                $notificationTemplate.addClass("blink_me_blue_bg");

            $notificationTemplate.removeClass("template");
            $notificationTemplate.click(() => $notificationTemplate.fadeOut(1000, () => $notificationTemplate.remove()));
            $('#notificationsBox > div').append($notificationTemplate);

            $notificationTemplate.show(1000, () => Shared.WaitSomeMs(30000).then(() => $notificationTemplate.fadeOut(1000, () => $notificationTemplate.remove())));

            if (status === "info") {
                if (isGuideNotification) {
                    let guideRing = new Audio(GuideInfoRing);
                    guideRing.play();
                } else {
                    let infoRing = new Audio(InfoRing);
                    infoRing.play();
                }
            }

            if (status === "success") {
                let successRing = new Audio(SuccessRing);
                successRing.play();
            }

            if (status === "danger") {
                let dangerRing = new Audio(DangerRing);
                dangerRing.play();
            }
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    // kolejka dla akcji. Wszystkie zapytania muszą się wykonywać po kolei ze względu na aktualizację stanu plantacji
    RequestsQueue: {
        queuedRequests: [],
        addRequest: function (req) {
            this.queuedRequests.push(req);
            $("#toCreateCount").text(this.queuedRequests.length);
            if (this.queuedRequests.length === 1)
                this.executeNextRequest();
        },
        clearQueue: function () {
            this.queuedRequests = [];
        },
        executeNextRequest: function () {
            const queuedRequests = this.queuedRequests;
            let timeBefore = new Date();
            console.log("Zaczynam procesowac zlecenie");
            queuedRequests[0]().then(() => {
                let timeAfter = new Date();
                console.log("Procesowanie zlecenia zakonczone - " + (timeAfter - timeBefore));
                queuedRequests.shift();
                $("#toCreateCount").text(this.queuedRequests.length);
                if (queuedRequests.length)
                    this.executeNextRequest();
            }).catch(err => {
                LogRedError(err);
                abp.message.error("Coś poszło nie tak ;(");
            });
        }
    }
};

Plantation.Init();
