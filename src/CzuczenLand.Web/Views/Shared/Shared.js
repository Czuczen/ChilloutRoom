console.log("shared.js obecny")

const Shared = {
    isMobile: false,
    Init: function () {
        String.prototype.toHHMMSS = function () {
            const sec_num = parseInt(this, 10);
            let hours = Math.floor(sec_num / 3600);
            let minutes = Math.floor((sec_num - (hours * 3600)) / 60);
            let seconds = sec_num - (hours * 3600) - (minutes * 60);

            if (hours < 10)
                hours = "0" + hours;
            
            if (minutes < 10) 
                minutes = "0" + minutes;
            
            if (seconds < 10) 
                seconds = "0" + seconds;
            
            return hours + ':' + minutes + ':' + seconds;
        }
        
        $(document).ready(() => {
            if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
                || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
                this.isMobile = true;
            }
            
            this.Chat.Init();
            this.ChatFocusableMakerInPopups();
        });
    },
    WaitSomeMs: (ms) => new Promise(resolve => setTimeout(resolve, ms)),
    FirstCharToLower: str => str[0].toLowerCase() + str.slice(1),
    FirstCharToUpper: str => str[0].toUpperCase() + str.slice(1),
    MapObjectsFirstCharKeyToLower: function (objects) {
        return objects.map(obj => Object.assign({}, ...Object.entries(obj).map(([k, v]) => ({[this.FirstCharToLower(k)]: v}))))
    },
    MapObjectsFirstCharKeyToUpper: function (objects) {
        return objects.map(obj => Object.assign({}, ...Object.entries(obj).map(([k, v]) => ({[this.FirstCharToUpper(k)]: v}))))
    },
    ConvertSecondsToDDHHMMSS: (seconds) => {
        const days = Math.floor(seconds / 86400);
        const hours = Math.floor((seconds % 86400) / 3600);
        const minutes = Math.floor((seconds % 3600) / 60);
        const secs = Math.floor(seconds % 60);

        const formattedDays = days.toString().padStart(2, '0');
        const formattedHours = hours.toString().padStart(2, '0');
        const formattedMinutes = minutes.toString().padStart(2, '0');
        const formattedSeconds = secs.toString().padStart(2, '0');

        return `${formattedDays}:${formattedHours}:${formattedMinutes}:${formattedSeconds}`;
    },
    GetDtIds: (ids) => {
        try {
            let dTIds = [];
            for (let i = 0; i < ids.length; i++) {
                const dtId = "#row_" + ids[i];
                dTIds.push(dtId);
            }

            return dTIds;
        } catch (ex) {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :<");
            return [];
        }
    },
    GameClock: () => setInterval(() => $("#gameClock").text(new Date().toISOString().replace("T", " ").split('.')[0]), 1000),
    MakeId: (length) => {
        let result = '';
        const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        const charactersLength = characters.length;

        for (let i = 0; i < length; i++)
            result += characters.charAt(Math.floor(Math.random() * charactersLength));

        return result;
    },
    ServicesLoader: (entityName) => {
        const appServices = abp.services.app;
        switch (entityName) {
            case "Configuration":
                return appServices.configuration;
            case "Tanant":
                return appServices.tenant;
            case "Role":
                return appServices.role;
            case "Session":
                return appServices.session;
            case "User":
                return appServices.user;
            case "Account":
                return appServices.account;
            case "DriedFruit":
                return appServices.driedFruit;
            case "Lamp":
                return appServices.lamp;
            case "Manure":
                return appServices.manure;
            case "Pot":
                return appServices.pot;
            case "Seed":
                return appServices.seed;
            case "Soil":
                return appServices.soil;
            case "Water":
                return appServices.water;
            case "News":
                return appServices.news;
            case "Drop":
                return appServices.drop;
            case "PlantationStorage":
                return appServices.plantationStorage;
            case "PlayerStorage":
                return appServices.playerStorage;
            case "Quest":
                return appServices.quest;
            case "Bonus":
                return appServices.bonus;
            case "Requirement":
                return appServices.requirement;
            case "District":
                return appServices.district;
            case "GeneratedType":
                return appServices.generatedType;
            case null:
                return null;
            default:
                return null;
        }
    },
    ChatFocusableMakerInPopups: () => {
        $(document).on("shown.bs.modal", "#showBlackMarketModal", () => {
            $(document).off('focusin.modal');
        });

        $(document).on("shown.bs.modal", "#editOrCreateModalBox", () => {
            $(document).off('focusin.modal');
            $("#editOrCreateModalBox").find('input[type=text]:visible').first().focus();
        });

        $(document).on("shown.bs.modal", "#structureTestsModal", () => {
            $(document).off('focusin.modal');
        });

        $(document).on("shown.bs.modal", "#showPlantationStorageModal", () => {
            $(document).off('focusin.modal');
        });
    },
    Slider: {
        slideIndex: 1,
        skipSlide: false,

        Init: function () {
            this.ShowSlides(this.slideIndex);
            $("#prevBtn").click(() => this.PlusSlides(-1));
            $("#nextBtn").click(() => this.PlusSlides(1));

            $(".dot").click(function () {
                const $this = $(this);
                const id = $this.attr("id")?.substring($this.attr("id")?.indexOf("-") + 1);
                Shared.Slider.CurrentSlide(parseInt(id));
            });
        },
        AutoSlide: function () {
            Shared.WaitSomeMs(15000).then(() => {
                if (!this.skipSlide) {
                    this.PlusSlides(1);
                    this.AutoSlide();
                } else {
                    this.skipSlide = false;
                    this.AutoSlide();
                }
            });
        },
        PlusSlides: function (n) {
            this.ShowSlides(this.slideIndex += n);
        },
        CurrentSlide: function (n) {
            this.slideIndex = n;
            this.ShowSlides(this.slideIndex);
        },
        ShowSlides: function (n) {
            const $slides = $(".mySlides");
            const $dots = $(".dot");

            if (n > $slides.length)
                this.slideIndex = 1;

            if (n < 1)
                this.slideIndex = $slides.length;

            $slides.each(function (index) {
                const $this = $(this);
                $this.hide(300);
                if (index === (Shared.Slider.slideIndex - 1))
                    $this.show(300);
            });

            $dots.each(function (index) {
                const $this = $(this);
                $this.removeClass("active");
                if (index === (Shared.Slider.slideIndex - 1))
                    $this.addClass("active");
            });

            Shared.WaitSomeMs(2000).then(() => this.ShowSlides);
        }
    },
    Chat: {
        chatHub: $.connection.chatHub,
        $chatBtn: $("#chatBtn"),
        $messageInput: $('#messageInput'),
        Init: function () {
            const $customersModal = $(".messages_right_bottom_position");
            this.$chatBtn.click(() => {
                $customersModal.toggle("show");
                this.$messageInput.focus();
            });

            $("#closeChat").click(() => $customersModal.toggle("show"));
            this.BindSendAction();
            this.InitSignalRHandlers();
        },
        InsertMessage: function (messageData) {
            const $message = $($(".message_template").clone().html());
            const $content = $message.find(".text");
            $message.addClass("right");
            const time = new Date().toLocaleTimeString();

            $("#sendMessageTime", $content).text(time);
            $("#userName", $content).text(messageData.userName);
            $("#messageBox", $content).text(messageData.message);

            $("#chatMessages").append($message);

            return setTimeout(() => {
                const $chatMessages = $("#chatMessages");
                $chatMessages.animate({scrollTop: $chatMessages.prop("scrollHeight")}, "slow");

                return $message.addClass('appeared');
            }, 50);
        },
        InitSignalRHandlers: function () {
            this.chatHub.client.receiveMessage = (messageData) => {
                this.$chatBtn.addClass("blink_me");
                Shared.WaitSomeMs(2000).then(() => this.$chatBtn.removeClass("blink_me"));
                this.InsertMessage(messageData);
            };

            this.chatHub.client.errorOccured = (ex) => {
                LogRedError(ex);
                abp.message.error("Coś poszło nie tak :((");
            };
        },
        BindSendAction: function () {
            this.$messageInput.keypress((event) => {
                const key = event.which;
                if (key === 13) {
                    const currMessage = this.$messageInput.val();
                    if (currMessage !== "") this.chatHub.server.sendMessageToAll(currMessage);
                    this.$messageInput.val("");
                    this.$messageInput.focus();
                }
            });

            $("#sendMessageBtn").click(() => {
                const currMessage = this.$messageInput.val();
                if (currMessage !== "")
                    this.chatHub.server.sendMessageToAll(currMessage);

                this.$messageInput.val("");
                this.$messageInput.focus();
            });
        }
    }
};

Shared.Init();
