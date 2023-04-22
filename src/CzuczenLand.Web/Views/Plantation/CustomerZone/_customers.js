console.log("_customers.js obecny");

Plantation.CustomerZone = {
    $customersBtn: $("#customersBtn"),
    Init: function () {
        try {
            const $customersModal = $(".customers_right_bottom_position");
            this.$customersBtn.click(() => $customersModal.toggle("show"));
            $("#closeCustomerZone").click(() => $customersModal.toggle("show"));
            this.BindSellAction();
            this.InitSignalRHandlers();
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    InsertMessage: function (offer) {
        try {
            const $message = $($(".offer_template").clone().html());
            $message.attr("id", offer.id);

            Shared.WaitSomeMs(offer.offerTime).then(() => $message.hide(300, () => $message.remove()));
            const message = "Siemka. Kupię " + offer.amount + offer.measureUnit + " " + offer.name + ". Cena: " + offer.buyPrice;
            $message.addClass("right").find('.text').html(message);
            $message.data("type_id", offer.typeId).data("amount", offer.amount);
            $("#customersMessages").append($message);
            return setTimeout(() => $message.addClass('appeared'), 50);
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
        }
    },
    InitSignalRHandlers: function () {
        const customersHub = Plantation.customersHub;
        customersHub.client.removeOffer = (guid) => {
            const $message = $("#" + guid);
            const $textBox = $("div.text", $message);
            $("div.btn", $message).remove();
            $textBox.text($textBox.text() + ". Sprzedano");
        };

        customersHub.client.getOffer = (offer) => {
            const selectedDistrictId = Plantation.$gameData.data("selected_district");
            const hasPlantation = Plantation.$gameData.data("has_plantation");

            if (selectedDistrictId === offer.districtId && hasPlantation) {
                this.InsertMessage(offer);
                this.$customersBtn.addClass("blink_me");
                Shared.WaitSomeMs(2000).then(() => this.$customersBtn.removeClass("blink_me"));
            }
        };
    },
    BindSellAction: () => {
        $("#customersMessages").click((event) => {
            try {
                const $target = $(event.target);
                if ($target.hasClass("btn btn-success") && !$target.data("clicked")) {
                    $target.data("clicked", true);
                    const $message = $target.closest(".message");
                    const offerId = $message.attr("id");
                    Plantation.RequestsQueue.addRequest(async () => {
                        const response = await Plantation.customersHub.server.sellDriedFruit($message.data("type_id"), $message.data("amount"), offerId);
                        if (response.status === "Error") {
                            $target.data("clicked", false);
                            Plantation.GenerateNotification("danger", [response.sellMessage]);
                        } else
                            Plantation.GenerateNotification("success", [response.sellMessage]);
                    });
                }
            } catch (ex) {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :<");
            }
        });
    }
};
