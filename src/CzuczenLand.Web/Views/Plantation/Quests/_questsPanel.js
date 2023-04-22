console.log("_questsPanel.js obecny")

Plantation.QuestsPanel = {
    $allQuestsBox: $("#allQuestsBox"),
    Init: function () {
        $.AdminBSB.rightSideBar.activate();
        this.InitSignalRHandlers();
        this.BindQuestsBtnsActions();
        this.SetQuestsBlinker();
    },
    SetQuestsBlinker: function () {
        try {
            this.$allQuestsBox.off('DOMSubtreeModified'); // na czas aktualizacji podświetlania wyłączamy event

            const playerGainedExp = parseFloat($("#playerGainedExp").data("gained_experience"));
            const $questsBtn = $("#questsBtn");
            const $completeQuestBtns = $(".complete-quest-btn");
            if (playerGainedExp > 0 && $completeQuestBtns.length) {
                $("li.tab-bookmark").removeClass("blink_me_blue_bg");
                $questsBtn.removeClass("blink_me_blue_bg");

                let inProgressLiToAddBlink = [];
                $completeQuestBtns.each(function () {
                    const $this = $(this);
                    const questId = $this.data("quest_id");
                    const $thisTabPane = $this.closest(".tab-pane");
                    const questType = $thisTabPane.attr("id").split("-").pop();

                    const $inProgressQuestsLi = $("#inProgressQuestsLi-" + questType);
                    const $btnCollapse = $("#btnCollapse-" + questId);

                    if ($this.css("display") !== "none") {
                        $questsBtn.addClass("blink_me_blue_bg");
                        inProgressLiToAddBlink.push($inProgressQuestsLi);
                        $btnCollapse.addClass("blink_me_blue_bg");
                    } else {
                        $inProgressQuestsLi.removeClass("blink_me_blue_bg");
                        $btnCollapse.removeClass("blink_me_blue_bg");
                    }
                });

                inProgressLiToAddBlink.forEach(item => item.addClass("blink_me_blue_bg"));
            } else if (!window.guideSwitcherActive) {
                $("a[data-target='#collapse-availableQuests'] > div").removeClass("blink_me_yellow_bg");
                $questsBtn.removeClass("blink_me_blue_bg");
                $("li.tab-bookmark").removeClass("blink_me_blue_bg");
            }

            this.$allQuestsBox.on("DOMSubtreeModified", () => this.SetQuestsBlinker());
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    BindQuestsBtnsActions: function () {
        $("#allQuestsBox").click((event) => {
            const $target = $(event.target);
            if ($target.hasClass("take-quest-btn")) {
                this.QuestAction.TakeQuestAction($target);
            } else if ($target.hasClass("complete-quest-btn")) {
                this.QuestAction.CompleteQuestAction($target);
            } else if ($target.hasClass("abandon-quest-btn")) {
                this.QuestAction.AbandonQuestAction($target);
            }
        });
    },
    InitSignalRHandlers: function () {
        const questHub = Plantation.questHub;
        questHub.client.notEnoughQuestToken = (questId, questType, message) => {
            Plantation.GenerateNotification("danger", message);
            this.QuestAction.GetAvailableQuestAction(questId, questType);
        };

        questHub.client.droppedQuest = (questId, questType) => {
            this.QuestAction.GetInProgressQuestAction(questId, questType);
        };

        questHub.client.setRestartedQuest = (questId, questType) => {
            try {
                const $questTab = $("#questTab-" + questId);
                $questTab.hide(500, () => {
                    $questTab.remove();
                    this.QuestAction.GetAvailableQuestAction(questId, questType);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };

        questHub.client.theLimitExpired = (questId, questType, message) => {
            this.QuestAction.GetAvailableQuestAction(questId, questType);
            abp.message.warn(message);
            new Audio(WarnRing).play();
        };

        questHub.client.setAvailableEventQuest = (questId, questType) => {
            try {
                const $questTab = $("#questTab-" + questId);
                $questTab.hide(500, () => {
                    $questTab.remove();
                    this.QuestAction.GetAvailableQuestAction(questId, questType);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };

        questHub.client.removeNotAvailableEventQuest = (questId, questType, isComplete) => {
            try {
                const $questTab = $("#questTab-" + questId);
                $questTab.hide(500, () => {
                    $questTab.remove();
                    this.QuestAction.GetEventQuestAction(questId, questType, isComplete);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };

        questHub.client.updateDuration = (data) => {
            try {
                const $remainingTime = $("#remainingTime-" + data.questId);
                $remainingTime.text(data.remainingTime);
                if (data.timeUp) {
                    this.QuestAction.AbandonTimeUpQuestAction(data.questId);
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };

        questHub.client.completedQuest = (data) => {
            try {
                if (!data.length)
                    abp.message.error("Brak nagród");
                else
                    Plantation.GenerateNotification("success", data);
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };

        questHub.client.updateQuests = (questsData) => {
            try {
                if (questsData === null || typeof questsData === "undefined" || questsData.length === 0) return;

                for (let i = 0; i < questsData.length; i++) {
                    const quest = questsData[i]
                    const questId = quest.questId;
                    const $questTab = $("#questTab-" + questId);

                    if (!$questTab.length) continue;
                    const $completeQuestBtn = $("#completeQuestBtn-" + questId);

                    if (quest.inProgress)
                        if (quest.questIsComplete)
                            $completeQuestBtn.show();
                        else
                            $completeQuestBtn.hide();

                    for (let j = 0; j < quest.updatingRequirements.length; j++) {
                        const req = quest.updatingRequirements[j];
                        const requirementId = req.requirementId;
                        const $reqBox = $("#reqBox-" + questId + "_" + requirementId);
                        $reqBox.css("width", req.requirementProgressPercentage);
                        $reqBox.text(req.requirementProgressText);
                    }
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        };
    },
    QuestAction: {
        ControllerActions: {
            CompleteQuest: "CompleteQuest",
            TakeQuest: "TakeQuest",
            AbandonQuest: "AbandonQuest",
            GetQuest: "GetQuest"
        },
        ActionTypes: {
            Get: "GET",
            Post: "POST"
        },
        TakeQuestAction: function ($takeQuestBtn) {
            try {
                if (!$takeQuestBtn.length) return;
                const questId = $takeQuestBtn.data("quest_id");
                const $questTab = $("#questTab-" + questId);
                const $questTabParent = $questTab.parent(".tab-pane");
                const questType = $questTabParent.attr("id").split("-").pop();
                const $boxToSetQuest = $("#inProgressQuests-" + questType);
                $questTab.hide(500, () => {
                    $questTab.remove();
                    this.ExecuteBtnAction(this.ControllerActions.TakeQuest, this.ActionTypes.Post, questId, $boxToSetQuest);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        },
        CompleteQuestAction: function ($completeQuestBtn) {
            try {
                if (!$completeQuestBtn.length) return;
                const questId = $completeQuestBtn.data("quest_id");
                const isRepetitive = JSON.parse($completeQuestBtn.data("is_repetitive").toLowerCase());
                const $questTab = $("#questTab-" + questId);
                const $questTabParent = $questTab.parent(".tab-pane");
                const questType = $questTabParent.attr("id").split("-").pop();

                let $boxToSetQuest;
                if (!isRepetitive)
                    $boxToSetQuest = $("#completedQuests-" + questType);
                else
                    $boxToSetQuest = $("#availableQuests-" + questType);

                $questTab.hide(500, () => {
                    $questTab.remove();
                    this.ExecuteBtnAction(this.ControllerActions.CompleteQuest, this.ActionTypes.Post, questId, $boxToSetQuest);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        },
        AbandonQuestAction: function ($abandonQuestBtn) {
            try {
                const questId = $abandonQuestBtn.data("quest_id");
                const $questTab = $("#questTab-" + questId);
                const $questTabParent = $questTab.parent(".tab-pane");
                const questType = $questTabParent.attr("id").split("-").pop();
                const $boxToSetQuest = $("#availableQuests-" + questType);
                $questTab.hide(500, () => {
                    $questTab.remove();
                    this.ExecuteBtnAction(this.ControllerActions.AbandonQuest, this.ActionTypes.Post, questId, $boxToSetQuest);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        },
        AbandonTimeUpQuestAction: function (questId) {
            try {
                const $questTab = $("#questTab-" + questId);
                const $questTabParent = $questTab.parent(".tab-pane");
                const questType = $questTabParent.attr("id").split("-").pop();
                const $boxToSetQuest = $("#availableQuests-" + questType);
                $questTab.hide(500, () => {
                    $questTab.remove();
                    this.ExecuteBtnAction(this.ControllerActions.GetQuest, this.ActionTypes.Get, questId, $boxToSetQuest);
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        },
        GetInProgressQuestAction: function (questId, questType) {
            const $boxToSetQuest = $("#inProgressQuests-" + questType);
            this.ExecuteBtnAction(this.ControllerActions.GetQuest, this.ActionTypes.Get, questId, $boxToSetQuest);
        },
        GetAvailableQuestAction: function (questId, questType) {
            const $boxToSetQuest = $("#availableQuests-" + questType);
            this.ExecuteBtnAction(this.ControllerActions.GetQuest, this.ActionTypes.Get, questId, $boxToSetQuest);
        },
        GetEventQuestAction: function (questId, questType, isComplete) {
            const $availableQuestsBox = $("#availableQuests-" + questType);
            const $completedQuestsBox = $("#completedQuests-" + questType);

            this.ExecuteBtnAction(this.ControllerActions.GetQuest, this.ActionTypes.Get, questId, isComplete ? $completedQuestsBox : $availableQuestsBox);
        },
        ExecuteBtnAction: (action, actionType, questId, $boxToSetQuest) => {
            try {
                Plantation.RequestsQueue.addRequest(async () => {
                    await new Promise(resolve => {
                        abp.ajax({
                            url: abp.appPath + "Plantation/" + action + "/" + questId,
                            type: actionType,
                            dataType: 'html',
                            success: (content) => {
                                if (content !== "") // puste jeśli limit zadań jest max. Informacja przyjdzie z huba
                                    $boxToSetQuest.append(content);
                            }
                        }).always(() => resolve());
                    });
                });
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        }
    }
};
