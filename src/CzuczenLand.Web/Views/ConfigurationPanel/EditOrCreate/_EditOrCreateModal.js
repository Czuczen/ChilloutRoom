console.log("_editOrCreateModal.js obecny");

ConfigurationPanel.EditOrCreate = {
    ActionCreate: "Create",
    ActionUpdate: "Update",
    ActionUpdateMany: "UpdateMany",
    $editOrCreateModal: $('#editOrCreateModal'),
    $editOrCreateModalContent: $("#editOrCreateModal div.modal-content"),

    Init: function () {
        try {
            $.validator.addMethod('multipleOf01', function (value, element) {
                return this.optional(element) || /^(?:\d+|\d*\.\d{0,2})$/.test(value);
            }, 'Wprowadź wielokrotność 0.01');

            const $form = $('form[name=editOrCreateForm]');
            $($form).validate({
                rules: {
                    inputNumber: {
                        multipleOf01: true
                    }
                },
                messages: {
                    inputNumber: {
                        multipleOf01: 'Wprowadź wielokrotność 0.01'
                    }
                }
            });

            Shared.WaitSomeMs(100).then(() => $form.find('input[type=text]:visible').first().focus());
            
            $form.closest('div.modal-content').find(".save-button").click((event) => {
                event.preventDefault();
                this.Save($form);
            });

            $form.find('input').on('keypress', (event) => {
                if (event.which === 13) {
                    event.preventDefault();
                    this.Save($form);
                }
            });

            $("input[type=checkbox]", $form).change(function () {
                try {
                    const $checkBox = $(this);
                    const input = $("#" + $checkBox.val());
                    if ($checkBox.is(":checked")) {
                        input.removeAttr("disabled");
                        input.focus();
                    } else {
                        input.val("");
                        input.focus();
                        input.attr("disabled", "disabled");
                    }
                } catch (ex) {
                    LogRedError(ex);
                    abp.message.error("Coś poszło nie tak :<");
                }
            });

            this.ActivateFieldsDependencies();

            $.AdminBSB.input.activate($form);
            const $dateTimeInputs = $("input[type=datetime-local]", $form);
            $dateTimeInputs.each(function () {$(this).parent().addClass("focused")});
            $dateTimeInputs.focusout(function () {Shared.WaitSomeMs(50).then(() => $(this).parent().addClass("focused"))});
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    InitConnectionSelectors: () => {
        $(".connectionSelector").each(function () {
            const $selector = $(this);
            const districtId = $selector.data("existing_connections_district_id");
            const entityName = $selector.attr("id").substring($selector.attr("id").indexOf("-") + 1);

            const select2Options = {
                ajax: {
                    url: abp.appPath + "ConfigurationPanel/GetSuggestions",
                    type: 'POST',
                    dataType: 'json',
                    delay: 250,
                    cache: false,
                    data: (params) => {
                        const generatedTypeId = $("#GeneratedTypeId").val();
                        if (typeof params.term !== "undefined")
                            return {
                                valueToSearch: params.term,
                                entity: entityName,
                                generatedTypeId: generatedTypeId,
                                districtId: districtId
                            };
                        else
                            return {
                                valueToSearch: "",
                                entity: entityName,
                                generatedTypeId: generatedTypeId,
                                districtId: districtId
                            };
                    },
                    processResults: (data, page) => {
                        if (typeof data !== "undefined")
                            return {results: data.result};
                    },
                    error: (ex) => {
                        if (ex.status === 0 && ex.readyState === 0 && ex.statusText === "abort") {
                            return null;
                        } else {
                            LogRedError(ex);
                            abp.message.error("Coś poszło nie tak :((");
                        }
                    }
                },
                width: 'style',
                language: "pl",
                allowClear: true,
                placeholder: "Wybierz " + $selector.data("hr_name"),
                minimumResultsForSearch: 1,
                multiple: true,
                disabled: false,
                // tags: true // jak włączone to z wyszukiwanej wartości robi opcję
            };

            $selector.select2(select2Options);
            ConfigurationPanel.EditOrCreate.SetExistingConnections($selector);
        });
    },
    Save: function ($form) {
        const $modal = this.$editOrCreateModal;
        abp.ui.setBusy($modal.val());
        try {
            const $enabledInputs = $(":input:visible:enabled:not([type='checkbox'])", $form);
            if (!$form.valid()) {
                let weHaveOne = false;
                $enabledInputs.each(function () {
                    const $this = $(this);
                    const $errorLabel = $this.next("label.error");
                    const display = $errorLabel.css("display");

                    if (display === "block" && !weHaveOne) {
                        $form.animate({scrollTop: $this.offset().top - 60}, 500);
                        $this.focus();
                        weHaveOne = true;
                    }
                });

                abp.ui.clearBusy($modal.val());
                return;
            } else if (!$enabledInputs.length) {
                abp.message.info("Nie wybrano żadnego pola");
                return;
            }

            const entity = ConfigurationPanel.$createBtn.val();
            const action = $("#crudAction").val();
            const $editManyAction = $("#editManyAction").val();
            const ids = $editManyAction === ConfigurationPanel.RecordAction.ActionsNames.EditVisible ? ConfigurationPanel.DataTableHelpers.GetVisibleRecordsIds() : null; // puste ids to update all
            $("input:hidden:not('#Id'), select:hidden", $form).remove(); // usuwamy ukryte pola przed serializacja
            const object = $form.serializeJSON();

            const connections = this.GetConnectedEntitiesIds();
            const objectWidthConnections = {Input: object, Connections: connections};

            const _currService = Shared.ServicesLoader(entity);
            switch (action) {
                case this.ActionCreate:
                    _currService.actionCreate(objectWidthConnections).done((res) => {
                        this.ExecuteResponse(res, this.ActionCreate);
                    }).always(() => abp.ui.clearBusy($modal.val()));
                    break;
                case this.ActionUpdate:
                    _currService.actionUpdate(objectWidthConnections).done((res) => {
                        this.ExecuteResponse(res, this.ActionUpdate);
                    }).always(() => abp.ui.clearBusy($modal.val()));
                    break;
                case this.ActionUpdateMany:
                    _currService.actionUpdateMany({fieldsToUpdate: object, ids: ids}).done((res) => {
                        this.ExecuteResponse(res, this.ActionUpdateMany);
                    }).always(() => abp.ui.clearBusy($modal.val()));
                    break;
            }
        } catch (ex) {
            LogRedError(ex);
            abp.ui.clearBusy($modal.val());
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    GetConnectedEntitiesIds: () => {
        let entityToIds = {};
        $(".connectionSelector").each(function () {
            const $this = $(this);
            const selectorData = $this.select2("data");
            let ids = [];
            $(selectorData).each(function () {
                ids.push(this.id);
            });

            entityToIds[$this.attr("id").substring($this.attr("id").indexOf("-") + 1)] = ids
        });

        return entityToIds;
    },
    GetNamesAndIds: (records, ids) => {
        try {
            let names = "";
            if (records.length) {
                for (let i = 0; i < records.length; i++) {
                    const id = records[i]?.id;
                    const name = records[i]?.name;
                    ids.push(id);

                    if (typeof name !== "undefined") {
                        names += "\r" + name + "\r";
                    }
                }
            }

            return names;
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    ExecuteResponse: function (res, action) {
        const $modal = this.$editOrCreateModal;
        if (res.infoMsg?.length) {
            abp.ui.clearBusy($modal.val());
            abp.message.info(res.infoMsg);
        } else {
            try {
                let ids = [];
                const names = this.GetNamesAndIds(res.records, ids);
                const dTIds = Shared.GetDtIds(ids);
                ConfigurationPanel.$dT.rows(dTIds).remove().draw();
                const validRes = Shared.MapObjectsFirstCharKeyToUpper(res.records);
                ConfigurationPanel.$dT.rows.add(validRes).draw();

                if ($modal.is(":visible"))
                    $("#showHideEditOrCreateModalFakeBtn").click();

                abp.ui.clearBusy($modal.val());
                abp.message.success(names, action === this.ActionCreate ? "Utworzono rekord" : action === this.ActionUpdate ? "Zaktualizowano rekord" : "Zaktualizowano rekordy");
            } catch (ex) {
                LogRedError(ex);
                abp.ui.clearBusy($modal.val());
                abp.message.error("Coś poszło nie tak :<");
            }
        }
    },
    GetExistingConnectionsOptionsList: (objectsList) => {
        try {
            const optionsList = [];
            for (const obj of objectsList) {
                optionsList.push(new Option(obj.Text, obj.Id, false, true));
            }

            return optionsList;
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
            return [];
        }
    },
    SetExistingConnections: function ($selector) {
        const existingConnections = $selector.data("existing_connections");
        if (existingConnections === "") return;
        let selectedIds = [];
        for (const obj of existingConnections) {
            selectedIds.push(obj.Id);
        }

        $selector.append(this.GetExistingConnectionsOptionsList(existingConnections)).val(selectedIds).trigger("change.select2");
    },
    // ========================= HARDCODE =================================
    // TODO: SPZ - Wrócić do tego jak będzie tworzony system pól zależnych
    // W przyszłości powstanie system pól zależnych, który będzie odpowiadał za pola zależne. Sprawdzić czy można by wykorzystać - INotifyPropertyChanged lub DependencyProperty
    ActivateFieldsDependencies: function () {
        const entity = $("#createButton").val();
        const $itemAmountBox = $("#ItemAmount").closest(".input-box");
        const $customEntityNameInput = $("#CustomEntityName");
        const $generatedTypeIdInput = $("#GeneratedTypeId");
        const $generatedTypeIdBox = $generatedTypeIdInput.closest(".input-box");
        const $questTypeInput = $("#QuestType");
        const $startTimeBox = $("#StartTime").closest(".input-box");
        const $endTimeBox = $("#EndTime").closest(".input-box");

        const $gold = $("#Gold");
        const $prestige = $("#Prestige");
        const $questToken = $("#QuestToken");
        const $dealerToken = $("#DealerToken");
        const $blackMarketToken = $("#BlackMarketToken");
        const $donToken = $("#DonToken");
        const $unlockToken = $("#UnlockToken");
        const $honor = $("#Honor");
        const $experience = $("#Experience");

        const $goldBox = $gold.closest(".input-box");
        const $prestigeBox = $prestige.closest(".input-box");
        const $questTokenBox = $questToken.closest(".input-box");
        const $dealerTokenBox = $dealerToken.closest(".input-box");
        const $blackMarketTokenBox = $blackMarketToken.closest(".input-box");
        const $donTokenBox = $donToken.closest(".input-box");
        const $unlockTokenBox = $unlockToken.closest(".input-box");
        const $honorBox = $honor.closest(".input-box");
        const $experienceBox = $experience.closest(".input-box");

        const ShowHideStartAndEndTime = ($this) => {
            const action = $("#crudAction").val();
            if ($this.val() !== "Event" && $this.length && action !== this.ActionUpdateMany) {
                $startTimeBox.hide();
                $endTimeBox.hide();
            } else {
                $startTimeBox.show();
                $endTimeBox.show();
            }
        }

        if ($itemAmountBox.length)
            $itemAmountBox.hide();

        ShowHideStartAndEndTime($questTypeInput);
        $questTypeInput.change(function () {
            ShowHideStartAndEndTime($(this));
        });

        $customEntityNameInput.change(function () {
            const $this = $(this);
            if ($this.val() === "" || $this.val() === "Plant" || $this.val() === "DriedFruit" || $this.val() === "Seed")
                $generatedTypeIdBox.show();
            else
                $generatedTypeIdBox.hide();
        });

        if (entity === "Drop") {
            if ($generatedTypeIdInput.val() !== "")
                $itemAmountBox.show();

            $prestige.on("input", function () {
                const $this = $(this);
                if ($this.val() !== "") {
                    $experienceBox.hide();
                    $goldBox.hide();
                    $generatedTypeIdBox.hide();
                } else {
                    $experienceBox.show();
                    $goldBox.show();
                    $generatedTypeIdBox.show();
                }
            });

            $experience.on("input", function () {
                const $this = $(this);
                if ($this.val() !== "") {
                    $prestigeBox.hide();
                    $goldBox.hide();
                    $generatedTypeIdBox.hide();
                } else {
                    $prestigeBox.show();
                    $goldBox.show();
                    $generatedTypeIdBox.show();
                }
            });

            $gold.on("input", function () {
                const $this = $(this);
                if ($this.val() !== "") {
                    $prestigeBox.hide();
                    $experienceBox.hide();
                    $generatedTypeIdBox.hide();
                } else {
                    $prestigeBox.show();
                    $experienceBox.show();
                    $generatedTypeIdBox.show();
                }
            });
        }

        const $connectionSelectorsBox = $("#connectionSelectors");
        $generatedTypeIdInput.change(function () {
            const $this = $(this);
            if ($connectionSelectorsBox.length) {
                if ($this.val() !== "") {
                    $connectionSelectorsBox.show();
                    ConfigurationPanel.EditOrCreate.InitConnectionSelectors(); // musi być inicjowany po show
                } else {
                    $connectionSelectorsBox.hide();
                }
            }

            if ($itemAmountBox.length)
                if ($this.val() !== "") {
                    $itemAmountBox.show();
                    $prestigeBox.hide();
                    $experienceBox.hide();
                    $goldBox.hide();
                    $questTokenBox.hide();
                    $dealerTokenBox.hide();
                    $blackMarketTokenBox.hide();
                    $donTokenBox.hide();
                    $unlockTokenBox.hide();
                    $honorBox.hide();
                } else {
                    $itemAmountBox.hide();
                    $prestigeBox.show();
                    $experienceBox.show();
                    $goldBox.show();
                    $questTokenBox.show();
                    $dealerTokenBox.show();
                    $blackMarketTokenBox.show();
                    $donTokenBox.show();
                    $unlockTokenBox.show();
                    $honorBox.show();
                }
        });

        const action = $("#crudAction").val();
        if (action === this.ActionUpdate) {
            $connectionSelectorsBox.show();
            this.InitConnectionSelectors();
        }
    }
};
