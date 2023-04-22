console.log("_plantationPanel.js obecny");

Plantation.PlantationPanel = {
    UnlockSlotTypes: {
        Artifact: "Artifact",
        Buff: "Buff",
        Daily: "Daily",
        Weekly: "Weekly",  
    },
    DonInfo: undefined,
    s2ProductsSelect: [],
    $productsBox: $(".product"),
    Init: function () {
        $("#customersBox").show(300);
        this.InitSelectProductsForNewPlant();
        this.InitCreatePlant();
        this.InitBonusesHandlers();
        this.StartRefreshingPlantsState();
        this.BindPlantActions();
        this.InitSlotsAdder();
        this.SetDonContext(Shared.MapObjectsFirstCharKeyToLower([Plantation.$gameData.data("don_data")])?.[0]);
    },
    InitSelectProductsForNewPlant: function () {
        this.$productsBox.each(function () {
            try {
                const $productBox = $(this);
                const select2Options = {
                    ajax: {
                        url: abp.appPath + "Plantation/GetPlayerProducts?entity=" + $productBox.attr("id"),
                        type: 'POST',
                        dataType: 'json',
                        delay: 250,
                        cache: false,
                        data: (params) => {
                            if (typeof params.term !== "undefined")
                                return {valueToSearch: params.term};
                            else
                                return {valueToSearch: ""};
                        },
                        processResults: (data, page) => {
                            if (typeof data !== "undefined")
                                return {results: data.result};
                        },
                        error: (ex) => {
                            if (ex.status === 0 && ex.readyState === 0 && ex.statusText === "abort")
                                return null;
                            else {
                                LogRedError(ex);
                                abp.message.error("Coś poszło nie tak :((");
                            }
                        }
                    },
                    theme: "classic",
                    width: 'resolve',
                    language: "pl",
                    placeholder: "Wybierz " + $productBox.data("hr_name"),
                    minimumResultsForSearch: 1,
                    multiple: false,
                    disabled: false,
                };

                const $s2ProductSelect = $productBox.select2(select2Options);
                $s2ProductSelect.on("select2:select", function (event) {
                    const productId = event.params.data.id;
                    $(".select2-selection__placeholder", "#select2-" + this.id + "-container").text(event.params.data.text).css("color", "whitesmoke");
                    Plantation.creatingPlantData[this.id] = productId;
                });

                Plantation.PlantationPanel.s2ProductsSelect.push($s2ProductSelect);
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    InitCreatePlant: function () {
        $("#createPlant").click(() => {
            try {
                if (Object.keys(Plantation.creatingPlantData)?.length === this.s2ProductsSelect?.length) {
                    Plantation.RequestsQueue.addRequest(async () => {
                        await new Promise(resolve => {
                            abp.ajax({
                                url: abp.appPath + "Plantation/CreatePlant",
                                data: JSON.stringify(Plantation.creatingPlantData),
                                type: 'post',
                                dataType: 'html',
                                success: (content) => {
                                    if (!content.includes("div")) {
                                        const jsonContent = JSON.parse(content);
                                        abp.message.warn(jsonContent?.result?.requirementMessage);
                                        let requirementRing = new Audio(WarnRing);
                                        requirementRing.play();
                                    } else {
                                        $(content).hide().appendTo($("#plantationPlants")).show(1000);
                                        let successRing = new Audio(SuccessRing);
                                        successRing.play();
                                    }
                                }
                            }).always(() => resolve());
                        });
                    });
                } else
                    abp.message.info("Wybierz wszystkie wymagane produkty")
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    InitGuideSwitcher: () => {
        try {
            const isNewPlayer = Plantation.$gameData.data("is_new_player");
            if (isNewPlayer) {
                Shared.WaitSomeMs(500).then(() => {
                    Plantation.GenerateNotification("info", ["Wybierz dzielnicę"], true);
                    $("#select2-s2DistrictSelector-container").addClass("blink_me_blue_bg");
                });
            }

            const playerGainedExp = parseFloat($("#playerGainedExp").data("gained_experience"));
            if (playerGainedExp > 0) return;

            const $playerPlants = $(".playerPlant");
            const $newPlantBox = $("#newPlant");

            if ($newPlantBox.length && !$playerPlants.length) {
                Shared.WaitSomeMs(500).then(() => {
                    Plantation.GenerateNotification("info", ["Wybierz produkty i stwórz roślinę"], true);
                    $newPlantBox.addClass("blink_me_blue_bg");
                });
            }

            $("#createPlant").one("click", async () => {
                const playerGainedExp = parseFloat($("#playerGainedExp").data("gained_experience"));
                if (playerGainedExp > 0) return;

                let plantIsNotVisible = true;
                while (plantIsNotVisible) {
                    const $plants = $(".playerPlant");
                    if ($plants.length) {
                        await Shared.WaitSomeMs(500);
                        Plantation.GenerateNotification("info", ["Rozpocznij dostępne zadanie"], true);

                        window.guideSwitcherActive = true;
                        const $availableQuestsBtn = $("a[data-target='#collapse-availableQuests'] > div");
                        const $availableQuestsLi = $("#availableQuestsLi-Others");
                        const $btnCollapse = $("#collapse-availableQuests").find('a[id^="btnCollapse-"]');

                        $availableQuestsBtn.addClass("blink_me_yellow_bg");
                        $availableQuestsLi.addClass("blink_me_blue_bg");
                        $btnCollapse.addClass("blink_me_blue_bg");

                        $btnCollapse.one("click", () => window.guideSwitcherActive = false);

                        $("#questsBtn").addClass("blink_me_blue_bg");
                        $newPlantBox.removeClass("blink_me_blue_bg");
                        plantIsNotVisible = false;
                    }

                    await Shared.WaitSomeMs(200);
                }
            });

            const $takeQuestBtns = $(".take-quest-btn");
            $takeQuestBtns.one("click", () => {
                Plantation.GenerateNotification("info", ['Zestaw porad znajdziesz pod ikoną "i" w prawym górnym rogu'], true);
                $("i", "#guideSlider").addClass("blink_me_yellow_bg");
                $("#questsBtn").removeClass("blink_me_blue_bg");
                $("#inProgressQuestsLi-Others > a").click();
            });

            $("i", "#guideSlider").on("click", function () {
                $(this).removeClass("blink_me_yellow_bg")
            });
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<")
        }
    },
    InitSlotsAdder: function () {
        const selectedDistrictId = Plantation.$gameData.data("selected_district");
        const _this = this;
        $("#addMaxBuffSlots").click(() => {
            try {
                abp.message.confirm(
                    "Chcesz odblokować miejsce?", "Odblokowanie!",
                    (isConfirmed) => {
                        if (isConfirmed) {
                            Plantation.RequestsQueue.addRequest(async () => {
                                const response = await Plantation.bonusHub.server.unlockBonusSlot(selectedDistrictId, _this.UnlockSlotTypes.Buff);
                                if (response.successfulActivation)
                                    Plantation.GenerateNotification("success", response.infoMessage);
                                else
                                    Plantation.GenerateNotification("danger", response.infoMessage);
                            });
                        }
                    }
                );
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });

        $("#addMaxArtifactsSlots").click(() => {
            try {
                abp.message.confirm(
                    "Chcesz odblokować miejsce?", "Odblokowanie!",
                    (isConfirmed) => {
                        if (isConfirmed) {
                            Plantation.RequestsQueue.addRequest(async () => {
                                const response = await Plantation.bonusHub.server.unlockBonusSlot(selectedDistrictId, _this.UnlockSlotTypes.Artifact);
                                if (response.successfulActivation)
                                    Plantation.GenerateNotification("success", response.infoMessage);
                                else
                                    Plantation.GenerateNotification("danger", response.infoMessage);
                            });
                        }
                    }
                );
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });

        $("#addMaxWeeklyQuestsCount").click(() => {
            try {
                abp.message.confirm(
                    "Chcesz odblokować miejsce?", "Odblokowanie!",
                    (isConfirmed) => {
                        if (isConfirmed) {
                            Plantation.RequestsQueue.addRequest(async () => {
                                const response = await Plantation.bonusHub.server.unlockBonusSlot(selectedDistrictId, _this.UnlockSlotTypes.Weekly);
                                if (response.successfulActivation)
                                    Plantation.GenerateNotification("success", response.infoMessage);
                                else
                                    Plantation.GenerateNotification("danger", response.infoMessage);
                            });
                        }
                    }
                );
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });

        $("#addMaxDailyQuestsCount").click(() => {
            try {
                abp.message.confirm(
                    "Chcesz odblokować miejsce?", "Odblokowanie!",
                    (isConfirmed) => {
                        if (isConfirmed) {
                            Plantation.RequestsQueue.addRequest(async () => {
                                const response = await Plantation.bonusHub.server.unlockBonusSlot(selectedDistrictId, _this.UnlockSlotTypes.Daily);
                                if (response.successfulActivation)
                                    Plantation.GenerateNotification("success", response.infoMessage);
                                else
                                    Plantation.GenerateNotification("danger", response.infoMessage);
                            });
                        }
                    }
                );
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    },
    SetDonContext: function (donData) {
        try {
            const selectedDistrictId = Plantation.$gameData.data("selected_district");
            const userId = Plantation.$gameData.data("user_id");

            if (donData.districtId === selectedDistrictId) {
                this.DonInfo = donData;
                const $donCrown = $("#donCrown");
                if (donData.weHaveDon && donData.donId === userId)
                    $donCrown.show();
                else
                    $donCrown.hide();
            }
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    BindPlantActions: function () {
        $("#plantationPlants").click(async (event) => {
            const $target = $(event.target);
            if ($target.hasClass("collectPlant"))
                await this.CollectPlantAction($target);
            else if ($target.hasClass("removePlant"))
                await this.RemovePlantAction($target);
        });
    },
    CollectPlantAction: async ($target) => {
        try {
            const plantId = $target.val();
            const $plantBox = $target.closest(".playerPlant");
            $plantBox.attr("id", ""); // żeby nie wysyłał do aktualizacji rośliny, która zaraz zniknie a później zostanie usunięta

            await new Promise(resolve => $plantBox.hide(1000, () => {
                $plantBox.remove();
                resolve();
            }));

            Plantation.RequestsQueue.addRequest(async () => {
                await new Promise(resolve => {
                    abp.ajax({
                        url: abp.appPath + "Plantation/CollectPlant/" + plantId,
                        type: 'post',
                        dataType: 'json',
                        success: (res) => Plantation.GenerateNotification("success", res)
                    }).always(() => resolve());
                });
            });
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    RemovePlantAction: async ($target) => {
        try {
            const plantId = $target.val();
            const $plantBox = $target.closest(".playerPlant");
            $plantBox.attr("id", ""); // żeby nie wysyłał do aktualizacji rośliny, która zaraz zniknie a później zostanie usunięta

            await new Promise(resolve => $plantBox.hide(1000, () => {
                $plantBox.remove();
                resolve();
            }));

            Plantation.RequestsQueue.addRequest(async () => {
                await new Promise(resolve => {
                    abp.ajax({
                        url: abp.appPath + "Plantation/RemovePlant/" + plantId,
                        type: 'delete',
                        dataType: 'json',
                        success: (res) => {
                            Plantation.GenerateNotification("danger", res);
                        }
                    }).always(() => resolve());
                });
            });
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    StartRefreshingPlantsState: function () {
        try {
            const $playerPlants = $(".playerPlant");

            let plantsIds = [];
            $playerPlants.each(function () {
                const plantId = $(this).attr("id").split("-").pop();
                if (plantId !== "") // jeśli puste to roślina właśnie została zebrana lub usunięta więc już jej nie aktualizujemy
                    plantsIds.push(plantId);
            });

            if (plantsIds.length) {
                $.connection.hub.start().done(() => {
                    Plantation.plantHub.server.refreshPlayerPlants(plantsIds).then(plants => {
                        try {
                            for (let i = 0; i < plants?.length; i++) {
                                const plant = plants[i];

                                const wiltLevel = plant.wiltLevel.toFixed(2);
                                const growingLevel = plant.growingLevel.toFixed(2);
                                const growingBarFill = ((growingLevel > (100 - wiltLevel)) ? (100 - wiltLevel) + "%" : growingLevel + "%");

                                const $growingBar = $("#growingBar-" + plant.plantId);
                                const $wiltBar = $("#wiltBar-" + plant.plantId);

                                const growingText = growingLevel.toString().replace(".", ",") + "%";
                                $growingBar.attr("aria-valuenow", growingLevel);
                                $growingBar.text(growingText);
                                $growingBar.css("width", growingBarFill)

                                $("#timeOfInsensitivity-" + plant.plantId).text(Shared.ConvertSecondsToDDHHMMSS(plant.timeOfInsensitivity));

                                const wiltText = wiltLevel.toString().replace(".", ",") + "%";
                                $wiltBar.attr("aria-valuenow", wiltLevel);
                                $wiltBar.text(wiltText);
                                $wiltBar.css("width", wiltLevel + "%")

                                if (wiltLevel === "100.00") {
                                    $wiltBar.removeClass("active");
                                    $("#collectPlant-" + plant.plantId).fadeOut(1000, () => $("#removePlant-" + plant.plantId).fadeIn(1000));
                                }

                                if (growingLevel === "100.00" && wiltLevel !== "100.00")
                                    $("#collectPlant-" + plant.plantId).fadeIn(1000);

                                if (wiltLevel === "100.00")
                                    $growingBar.text("");

                                if (growingLevel === "100.00")
                                    $growingBar.removeClass("active");

                                if (plant.wiltLevel > 0) {
                                    $growingBar.removeClass("active");
                                    $wiltBar.css("visibility", "visible")
                                }

                                const $timeRemainingSpan = $("#timeRemainingSpan-" + plant.plantId);
                                $timeRemainingSpan.text(Shared.ConvertSecondsToDDHHMMSS(plant.timeRemaining));
                            }
                        } catch (ex) {
                            LogRedError(ex);
                            abp.message.error("Coś poszło nie tak :<");
                        }
                    });
                });
            }
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }

        Shared.WaitSomeMs(1000).then(() => this.StartRefreshingPlantsState());
    },
    InitBonusesHandlers: () => {
        let buffsQueueInProgress = false;
        let artifactsQueueInProgress = false;
        Plantation.bonusHub.client.bonusUpdate = (data) => {
            try {
                const $activeBonus = $("#activeBonus-" + data.bonusId);
                if (data.isArtifact) {
                    if (data.isActive) {
                        if (!$activeBonus.length) {
                            const $artifactsBox = $("#artifactsBox");
                            if (!artifactsQueueInProgress) // nim został zrenderowany przychodził kilka razy i renderowało się kilka
                            {
                                artifactsQueueInProgress = true;
                                Plantation.RequestsQueue.addRequest(async () => {
                                    await new Promise(resolve => {
                                        abp.ajax({
                                            url: abp.appPath + "Plantation/GetBonus/" + data.bonusId,
                                            type: 'GET',
                                            dataType: 'html',
                                            success: (res) => $artifactsBox.append(res)
                                        }).always(() => {
                                            artifactsQueueInProgress = false;
                                            resolve()
                                        });
                                    });
                                });
                            }
                        }
                    } else
                        $activeBonus.closest(".show-popover").remove();
                } else {
                    if (data.isActive) {
                        if ($activeBonus.length)
                            $("#remainingActiveTimeBuff-" + data.bonusId).text("Pozostały czas: " + Shared.ConvertSecondsToDDHHMMSS(data.remainingActiveTime));
                        else {
                            const $buffsBox = $("#buffsBox");
                            if (!buffsQueueInProgress) // nim został zrenderowany przychodził kilka razy i renderowało się kilka
                            {
                                buffsQueueInProgress = true;
                                Plantation.RequestsQueue.addRequest(async () => {
                                    await new Promise(resolve => {
                                        abp.ajax({
                                            url: abp.appPath + "Plantation/GetBonus/" + data.bonusId,
                                            type: 'GET',
                                            dataType: 'html',
                                            success: (res) => $buffsBox.append(res)
                                        }).always(() => {
                                            buffsQueueInProgress = false;
                                            resolve()
                                        });
                                    });
                                });
                            }
                        }
                    } else
                        $activeBonus.closest(".show-popover").remove();
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };
    }
};
