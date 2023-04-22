console.log("TicTacToe index.js obecny");

const TimeToAutoMove = 10;

const Tied = "Tied";
const Win = "Win";
const Lose = "Lose";

const Easy = "Easy";
const Normal = "Normal";
const Hard = "Hard";
const Insane = "Insane";

const ButtonsAdjacentToCenter = [ "cell01", "cell12", "cell21", "cell10"]
const CornerBtns = ["cell00", "cell02", "cell20", "cell22"];

let playerSymbol,
    opponentSymbol,
    win,
    turn,
    row, 
    column,
    cpuEnabled = true,
    groupName,
    connectedPlayersCount = 0,
    playerMadeMove = false,
    opponentMadeMove = false,
    gameRestarted = false,
    gameInProgress = false,
    playerTurn = true,
    cpuCheckAllowedClick = false,
    cpuDifficulty = Easy;

const ticTacToeHub = $.connection.ticTacToeHub;

$(window).on('beforeunload', () => {if (gameInProgress) return ""});

$(document).ready(() => 
{
    BindHubActions();
    BindChooseHumanAction();
    BindChooseCpuAction();
    BindSelectFieldAction();
});

const BindHubActions = () =>
{
    try
    {
        ticTacToeHub.client.errorOccured = (ex) =>
        {
            LogRedError(ex);
            abp.message.error("Coś poszło nie tak :((");
        };

        ticTacToeHub.client.mustWaitForOpponent = () =>
        {
            $("#enemy-screen").fadeOut(300, () => $("#wait-for-enemy").fadeIn(300));
        };

        ticTacToeHub.client.opponentFound = (opponentConnectionId) =>
        {
            ConfirmationOpponent(opponentConnectionId);
        };

        ticTacToeHub.client.opponentRejectMatch = () =>
        {
            abp.message.info("Przeciwnik odrzucił rozgrywkę")
            $("#wait-for-enemy-choice").fadeOut(300, () => $("#wait-for-enemy").fadeIn(300));
        };

        ticTacToeHub.client.mustWaitForOpponentChoice = () =>
        {
            $("#wait-for-enemy-choice").fadeIn(300);
        };

        ticTacToeHub.client.matchStarted = (nameGroup, opponentData) =>
        {
            RestartGame();
            gameRestarted = false;
            cpuEnabled = false;
            gameInProgress = true;
            groupName = nameGroup;
            const $gameScreen = $("#game-screen");

            $("#wait-for-enemy").fadeOut(300, () => $gameScreen.fadeIn(300));
            $("#wait-for-enemy-choice").fadeOut(300, () => $gameScreen.fadeIn(300));

            SetPlayersSymbols(opponentData.symbol);
            SetOpponentInformation(opponentData);
            $(".opponent-information").fadeIn(300);
            ScrollToTableIfIsMobile();
            SetPlayerTurnPossibility(null, playerSymbol === "X");
        };

        ticTacToeHub.client.setMove = (elementId, symbol) =>
        {
            InsertSymbol(elementId, symbol).then( async () => await SetPlayerTurnPossibility(symbol, symbol === opponentSymbol));
        };

        ticTacToeHub.client.usersCount = (count) =>
        {
            connectedPlayersCount = count;
            $("#playersOnline").text(connectedPlayersCount);
        };

        ticTacToeHub.client.opponentDisconnectedFromQueue = () =>
        {
            abp.message.info("Przeciwnik wyszedł z kolejki");
            $("#wait-for-enemy-choice").fadeOut(300, () => $("#wait-for-enemy").fadeIn(300));
        };

        ticTacToeHub.client.opponentDisconnected = () =>
        {
            abp.message.success("Przeciwnik wyszedł z gry", "Wygrana");
            SetEndMatchResponse(Win);
        };
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const BindSelectFieldAction = () => 
{
    try 
    {
        $(".cell").click( async function()
        {
            if(!win && this.innerHTML === "")
            {
                LockFields();
                if (!cpuEnabled)
                {
                    ticTacToeHub.server.makeMove(groupName, this.id);
                }
                else
                {
                    await MakeMoveWithTheComputer(this.id);
                }
            }
        });
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const BindChooseCpuAction = () => 
{
    try 
    {
        $("#choose-cpu").one("click", () =>
        {
            const $choseHuman = $("#choose-human");

            $("#cpu-difficulty-level-easy").click(() =>
            {
                cpuDifficulty = Easy;

                $choseHuman.attr("disabled", "disabled");
                Shared.WaitSomeMs(1000).then(() => $choseHuman.removeAttr("disabled"));

                $("#enemy-screen").fadeOut(300, () => StartGameWithCpu());
            });

            $("#cpu-difficulty-level-normal").click(() =>
            {
                cpuDifficulty = Normal;

                $choseHuman.attr("disabled", "disabled");
                Shared.WaitSomeMs(1000).then(() => $choseHuman.removeAttr("disabled"));

                $("#enemy-screen").fadeOut(300, () => StartGameWithCpu());
            });

            $("#cpu-difficulty-level-hard").click(() =>
            {
                cpuDifficulty = Hard;

                $choseHuman.attr("disabled", "disabled");
                Shared.WaitSomeMs(1000).then(() => $choseHuman.removeAttr("disabled"));

                $("#enemy-screen").fadeOut(300, () => StartGameWithCpu());
            });

            $("#cpu-difficulty-level-insane").click(() =>
            {
                cpuDifficulty = Insane;

                $choseHuman.attr("disabled", "disabled");
                Shared.WaitSomeMs(1000).then(() => $choseHuman.removeAttr("disabled"));

                $("#enemy-screen").fadeOut(300, () => StartGameWithCpu());
            });
        });
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const BindChooseHumanAction = () => 
{
    try 
    {
        $("#choose-human").click( () =>
        {
            const $choseCpu = $("#choose-cpu");
            $choseCpu.attr("disabled", "disabled");
            Shared.WaitSomeMs(1000).then(() => $choseCpu.removeAttr("disabled"));

            $("#enemy-screen").fadeOut(300);
            ticTacToeHub.server.setInQueueRoom();
            $("#cancelWaitForOpponent").one("click", () =>
            {
                ticTacToeHub.server.cancelWaitForOpponent();
                $("#wait-for-enemy").fadeOut(300, () => $("#enemy-screen").fadeIn(300));
            });
        });
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const StartGameWithCpu = () => 
{
    try 
    {
        RestartGame();
        cpuEnabled = true;
        SetSymbolsWithCpuPlay();

        $("#playerTimeToMove").hide();
        $("#opponentTimeToMove").hide();

        $("#game-screen").fadeIn(300, () =>
        {
            playerTurn = playerSymbol === "X";

            if (!playerTurn)
            {
                // pierwszy ruch krzyżyka
                const elementId = GetFirstMoveElementId();
                MakeMoveWithTheComputer(elementId);
            }
            else
            {
                SetPlayerTurnPossibilityAgainstCpu(playerTurn);
            }
        });

        ScrollToTableIfIsMobile();
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const ScrollToTableIfIsMobile = () => 
{
    if (Shared.isMobile)
        $("#ticTacToeTable")[0].scrollIntoView({behavior: "smooth"});
};

const GetFirstMoveElementId = () => 
{
    let elementId = "";
    
    const cornerIndex = Math.floor(Math.random() * (4 - 0 + 1) + 0);
    if (cornerIndex === 4)
    {
        elementId = "cell11";
    }
    else
    {
        elementId = CornerBtns[cornerIndex];
    }
    
    return elementId;
};

const MakeMoveWithTheComputer = async (elementId) => 
{
    try 
    {
        if (playerTurn)
        {
            await InsertSymbol(elementId, playerSymbol);
            SetPlayerTurnPossibilityAgainstCpu(false);
            playerTurn = false;
            if (!win && turn < 9) CpuTurn();
        }
        else
        {
            await InsertSymbol(elementId, opponentSymbol);
            SetPlayerTurnPossibilityAgainstCpu(true);
            playerTurn = true;
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const ConfirmationOpponent = async (opponentConnectionId) => 
{
    try 
    {
        $("#wait-for-enemy").fadeOut(300);

        const waitLimit = 5
        let waitCount = 5;

        abp.message.confirm("Rozpocząć rozgrywkę?", "Przeciwnik znaleziony", (confirm) =>
        {
            if (confirm)
            {
                ticTacToeHub.server.startMatch(opponentConnectionId);
            }
            else
            {
                ticTacToeHub.server.matchRejected(opponentConnectionId);
                $("#enemy-screen").fadeIn(300);
            }
        });

        const $okBtn = $(".swal-button--confirm");
        $okBtn.text("Ok " + waitCount);
        const $cancelBtn = $(".swal-button--cancel");

        for (let i = 0; i < waitLimit; i++)
        {
            if (!$cancelBtn.is(":visible")) break;
            await Shared.WaitSomeMs(1000);
            waitCount--;
            $okBtn.text("Ok " + waitCount);
        }

        if ($cancelBtn.is(":visible"))
        {
            $cancelBtn.click();
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const InsertSymbol = async (elementId, symbol) => 
{
    try 
    {
        turn++;
        const $element =  $("#" + elementId);
        $element.text(symbol);
        $element.addClass("cannot-use"); // potrzebna ta klasa cannot-use ???
        if(symbol === opponentSymbol) $element.addClass("player-two");
        CheckWinConditions(elementId, symbol);

        if(win || turn > 8)
        {
            LockFields();
            await Shared.WaitSomeMs(1000);

            if (!cpuEnabled)
            {
                await ExecuteEndMatch(symbol, win);
            }
            else
            {
                if (!win) abp.message.info("Remis");
                if (win && playerTurn) abp.message.success("Wygrana");
                if (win && !playerTurn) abp.message.error("Przegrana");

                $("#game-screen").fadeOut(300, () => $("#enemy-screen").fadeIn(300));
            }

            RestartGame();
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
}

const RestartGame = ()  =>
{
    try 
    {
        turn = 0;
        win = false;
        playerMadeMove = false;
        opponentMadeMove = false;
        gameRestarted = true;

        const $playerTimeToMove = $("#playerTimeToMove");
        const $opponentTimeToMove = $("#opponentTimeToMove");

        $playerTimeToMove.text("-");
        $opponentTimeToMove.text("-");
        $playerTimeToMove.show();
        $opponentTimeToMove.show();

        $(".cell").text("");
        $(".cell").removeClass("win-cell");
        $(".cell").removeClass("cannot-use");
        $(".cell").removeClass("player-two");   
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
}

const SetPlayersSymbols = (symbol) =>
{
    try 
    {
        opponentSymbol = symbol;
        playerSymbol = symbol === "X" ? "O" : "X";

        $("#playerSymbol").text(playerSymbol);
        $("#opponentSymbol").text(opponentSymbol);   
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
}

const SetSymbolsWithCpuPlay = () => 
{
    try 
    {
        const symbolNumber = Math.floor(Math.random() * 2) + 1;
        playerSymbol = symbolNumber === 1 ? "X" : "O";
        opponentSymbol = symbolNumber === 2 ? "X" : "O";

        $("#playerSymbol").text(playerSymbol);
        $("#opponentSymbol").text(opponentSymbol);   
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
}

const SetPlayerTurnPossibility = async (symbol, possibility) =>
{
    try 
    {
        playerMadeMove = symbol === playerSymbol;
        opponentMadeMove = symbol === opponentSymbol
        if (possibility)
        {
            UnlockFields();
            await CountDownPlayerTimeToMove();
        }
        else
        {
            LockFields();
            await CountDownOpponentTimeToMove();
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const SetPlayerTurnPossibilityAgainstCpu = (possibility) => 
{
    try 
    {
        if (possibility)
        {
            UnlockFields();
        }
        else
        {
            LockFields();
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const CountDownOpponentTimeToMove = async () =>
{
    try 
    {
        $("#playerTimeToMove").text("-");
        let timeLeftToMove = TimeToAutoMove;
        const $opponentTimeToMove = $("#opponentTimeToMove");
        $opponentTimeToMove.text(timeLeftToMove);
        const currTurn = turn;
        
        for (let i = 0; i < TimeToAutoMove; i++)
        {
            if (gameRestarted || currTurn !== turn) return;
            
            if (!opponentMadeMove)
            {
                $opponentTimeToMove.text(timeLeftToMove);
                timeLeftToMove--;
            }
            else
            {
                $opponentTimeToMove.text("-");
                return;
            }

            await Shared.WaitSomeMs(1000);
        }

        $opponentTimeToMove.text(timeLeftToMove);
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const CountDownPlayerTimeToMove = async () => 
{
    try 
    {
        $("#opponentTimeToMove").text("-");
        const $playerTimeToMove = $("#playerTimeToMove");
        $playerTimeToMove.one("cpu-must-move", () => CpuTurn());
        let timeLeftToMove = TimeToAutoMove;
        $playerTimeToMove.text(timeLeftToMove);
        const currTurn = turn;

        for (let i = 0; i < TimeToAutoMove; i++)
        {
            if (gameRestarted || currTurn !== turn) return $playerTimeToMove.off("cpu-must-move");
            
            if (!playerMadeMove)
            {
                $playerTimeToMove.text(timeLeftToMove);
                timeLeftToMove--;
            }
            else
            {
                $playerTimeToMove.text("-");
                return $playerTimeToMove.off("cpu-must-move");
            }

            await Shared.WaitSomeMs(1000);
        }

        $playerTimeToMove.text(timeLeftToMove);
        
        if (!playerMadeMove)
        {
            $playerTimeToMove.trigger("cpu-must-move");
        }

        $playerTimeToMove.off("cpu-must-move");
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const SetOpponentInformation = (opponentData) => 
{
    try 
    {
        $("#opponentPlayerName > h4").text(opponentData.playerName);
        $("#opponentGamesPlayed > span").text(opponentData.gamesPlayed);
        $("#opponentGamesWon > span").text(opponentData.gamesWon);
        $("#opponentGamesLost > span").text(opponentData.gamesLost);
        $("#opponentTiedGames > span").text(opponentData.tiedGames);
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const ExecuteEndMatch = async (symbol, win) =>
{
    if (!win)
    {
        abp.message.info("Remis");
        await SetEndMatchResponse(Tied);
    }
    else
    {
        if (playerSymbol === symbol)
        {
            abp.message.success("Wygrana");
            await SetEndMatchResponse(Win);
        }
        else
        {
            abp.message.error("Przegrana");
            await SetEndMatchResponse(Lose);
        }
    }
};

const LockFields = () => $(".cell").attr("disabled", "disabled");
const UnlockFields = () => $(".cell").removeAttr("disabled");

const SetEndMatchResponse = async (matchResult) => 
{
    gameInProgress = false;
    try 
    {
        const $gameScreen = $("#game-screen");
        const $opponentInformation = $(".opponent-information");
        const $enemyScreen = $("#enemy-screen");
        abp.ui.setBusy($gameScreen);

        const storage = await ticTacToeHub.server.endMatch(matchResult, false, null);

        $("#playerPlayedGames > span").text(storage.gamesPlayed);
        $("#playerWinGames > span").text(storage.gamesWon);
        $("#playerLostGames > span").text(storage.gamesLost);
        $("#playerDrawsGames > span").text(storage.tiedGames);

        abp.ui.clearBusy($gameScreen);
        $opponentInformation.fadeOut(300);
        $gameScreen.fadeOut(300, () => $enemyScreen.fadeIn(300));
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const CheckWinConditions = (elementId, elementInnerHTML) =>
{
    try 
    {
        row = elementId[4];
        column = elementId[5];

        const cellNumbers1 = CheckClickedColumnWinPossibility(elementInnerHTML);
        WinHighlight(cellNumbers1);
        if(win) return;

        const cellNumbers2 = CheckClickedRowWinPossibility(elementInnerHTML);
        WinHighlight(cellNumbers2);
        if(win) return;

        const cellNumbers3 = CheckFirstDiagonalWinPossibility(elementInnerHTML);
        WinHighlight(cellNumbers3)
        if(win) return;

        const cellNumbers4 = CheckSecondDiagonalWinPossibility(elementInnerHTML);
        WinHighlight(cellNumbers4);
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
}

// DRUGA PRZEKĄTNA (od prawy górny do lewy dolny)
const CheckSecondDiagonalWinPossibility = (elementInnerHTML) =>
{
    let cellNumbers = [];
    try 
    {
        win = false;
        if($("#cell02").text() === elementInnerHTML)
        {
            if($("#cell11").text() === elementInnerHTML)
            {
                if($("#cell20").text() === elementInnerHTML)
                {
                    win = true;

                    cellNumbers.push("02");
                    cellNumbers.push("11");
                    cellNumbers.push("20");
                }
            }
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }

    return cellNumbers;
};

// PIERWSZA PRZEKĄTNA (od lewy górny do prawy dolny)
const CheckFirstDiagonalWinPossibility = (elementInnerHTML) =>
{
    let cellNumbers = [];
    try 
    {
        win = true;
        for(let i = 0; i < 3; i++)
        {
            const numbers = i.toString() + i.toString();
            cellNumbers.push(numbers);

            if($("#cell" + i + i).text() !== elementInnerHTML)
            {
                win = false;
            }
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }

    return cellNumbers;
}

// POZIOMA (sprawdź wiersz klikniętej komórki)
const CheckClickedRowWinPossibility = (elementInnerHTML) =>
{
    let cellNumbers = [];
    try 
    {
        win = true;
        for(let i = 0; i < 3; i++)
        {
            const numbers = row.toString() + i.toString();
            cellNumbers.push(numbers);

            if($("#cell" + row + i).text() !== elementInnerHTML)
            {
                win = false;
            }
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }

    return cellNumbers;
};

// PIONOWY (sprawdź czy wszystkie symbole w kolumnie klikniętej komórki są takie same)
const CheckClickedColumnWinPossibility = (elementInnerHTML) =>
{
    let cellNumbers = [];
    try 
    {
        win = true;
        for(let i = 0; i < 3; i++)
        {
            const numbers = i.toString() + column.toString();
            cellNumbers.push(numbers);

            if($("#cell" + i + column).text() !== elementInnerHTML)
            {
                win = false;
            }
        }    
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
    
    return cellNumbers;
}

const WinHighlight = (cellNumbers) =>
{
    try 
    {
        if(win)
        {
            for(let i = 0; i < cellNumbers.length; i++)
            {
                $("#cell" + cellNumbers[i]).addClass("win-cell");
            }
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
};

const CpuTurn = () =>
{
    cpuCheckAllowedClick = false;
    try 
    {
        const $cell11 = $("#cell11");
        if (cpuDifficulty === Hard || cpuDifficulty === Insane)
        {
            // pierwszy ruch kółka - defensywne przygotowanie
            if (opponentSymbol === "O" && turn === 1 && $cell11.text() !== "")
            {
                const cornerIndex = Math.floor(Math.random() * (3 - 0 + 1) + 0);
                const elementId = CornerBtns[cornerIndex];
                $("#" + elementId).click();
                return;
            }

            // pierwszy ruch kółka - zajmujemy środek jeśli to tylko możliwe by później móc stwarzać zagrorzenie 3x
            if (opponentSymbol === "O" && turn === 1 && $cell11.text() === "")
            {
                $cell11.click();
                return;
            }

            // drugi ruch krzyżyka - jeśli środek został zablokowany - zaznacz po przekątnej
            if (opponentSymbol === "X" && turn === 2 && $cell11.text() === "O")
            {
                const $cell02 = $("#cell02");
                const $cell22 = $("#cell22");
                const $cell20 = $("#cell20");
                const $cell00 = $("#cell00");

                if ($cell02.text() === "X") return $cell20.click();
                if ($cell22.text() === "X") return $cell00.click();
                if ($cell20.text() === "X") return $cell02.click();
                if ($cell00.text() === "X") return $cell22.click();
            }
        }

        if (cpuDifficulty === Insane)
        {
            // drugi ruch kółka - blokada pułapki na dwa X po rogach zblokowane środkowym kółkiem (jeśli środek wolny kółko wcześniej go zajmie)
            if (opponentSymbol === "O" && turn === 3)
            {
                let playerSymbolsInCorner = 0;
                for (let i = 0; i < CornerBtns.length; i++)
                {
                    const corner = CornerBtns[i];
                    if ($("#" + corner).text() === "X")
                    {
                        playerSymbolsInCorner++;
                    }
                }

                // jeśli są dwa X po rogach to zaznacz przyległy do środka
                if (playerSymbolsInCorner === 2)
                {
                    const adjacentIndex = Math.floor(Math.random() * (3 - 0 + 1) + 0);
                    const adjacentBtn = ButtonsAdjacentToCenter[adjacentIndex];
                    $("#" + adjacentBtn).click();
                    return;
                }
            }

            // drugi ruch kółka - blokada pułapki na dwa X przy rogu - wstaw znak w róg pułapki
            if (opponentSymbol === "O" && turn === 3)
            {
                const $cell01 = $("#cell01");
                const $cell12 = $("#cell12");
                const $cell21 = $("#cell21");
                const $cell10 = $("#cell10");

                const $cell02 = $("#cell02");
                const $cell22 = $("#cell22");
                const $cell20 = $("#cell20");
                const $cell00 = $("#cell00");

                if ($cell01.text() === "X" && $cell12.text() === "X") return $cell02.click();
                if ($cell12.text() === "X" && $cell21.text() === "X") return $cell22.click();
                if ($cell21.text() === "X" && $cell10.text() === "X") return $cell20.click();
                if ($cell10.text() === "X" && $cell01.text() === "X") return $cell00.click();
            }

            // drugi ruch kółka - blokada pułapki X środek a drugi na zablokowaną już możliwość 3x - zaznacz przeciwległe
            if (opponentSymbol === "O" && turn === 3)
            {
                if ($cell11.text() === "X")
                {
                    for (let i = 0; i < CornerBtns.length; i++)
                    {
                        const currCellId = CornerBtns[i];
                        if ($("#" + currCellId).text() === "X")
                        {
                            if (currCellId === "cell02" && $("#cell20").text() === "O")
                            {
                                $("#cell22").click();
                                return;
                            }

                            if (currCellId === "cell22" && $("#cell00").text() === "O")
                            {
                                $("#cell02").click();
                                return;
                            }

                            if (currCellId === "cell00" && $("#cell22").text() === "O")
                            {
                                $("#cell20").click();
                                return;
                            }

                            if (currCellId === "cell20" && $("#cell02").text() === "O")
                            {
                                $("#cell00").click();
                                return;
                            }
                        }
                    }
                }
            }

            // drugi ruch krzyżyka - jeśli środek nie został zablokowany
            if (opponentSymbol === "X" && turn === 2 && $cell11.text() !== "O")
            {
                // jeśli któreś z przyległych pól do środka jest zaznaczone - zaznacz środek
                for (let i = 0; i < ButtonsAdjacentToCenter.length; i++)
                {
                    const cellId = ButtonsAdjacentToCenter[i]
                    if ($("#" + cellId).text() === "O")
                    {
                        $cell11.click();
                        return;
                    }
                }

                // zaznaczamy dowolny pusty narożnik jeśli żadne przyległe nie jest zaznaczone
                for (let i = 0; i < CornerBtns.length; i++)
                {
                    const cellId = CornerBtns[i]
                    const $corner = $("#" + cellId);
                    if ($corner.text() === "")
                    {
                        $corner.click();
                        return;
                    }
                }
            }

            // drugi ruch krzyżyka - jeśli róg został zablokowany - zaznacz róg po przekątnej
            if (opponentSymbol === "X" && turn === 2 && $cell11.text() === "X")
            {
                for (let i = 0; i < CornerBtns.length; i++)
                {
                    const currCellId = CornerBtns[i];
                    if ($("#" + currCellId).text() === "O")
                    {
                        if (currCellId === "cell02")
                        {
                            $("#cell20").click();
                            return;
                        }

                        if (currCellId === "cell22")
                        {
                            $("#cell00").click();
                            return;
                        }

                        if (currCellId === "cell00")
                        {
                            $("#cell22").click();
                            return;
                        }

                        if (currCellId === "cell20")
                        {
                            $("#cell02").click();
                            return;
                        }
                    }
                }
            }

            // drugi ruch kółka - jeśli krzyżyk jest w narożniku i przyległej 
            if (opponentSymbol === "O" && turn === 3 && $cell11.text() === "O")
            {
                let adjacentId = "";
                let cornerId = "";

                for (let i = 0; i < CornerBtns.length; i++)
                {
                    const cellId = CornerBtns[i]
                    if ($("#" + cellId).text() === "X")
                    {
                        cornerId = cellId;
                        break;
                    }
                }

                for (let i = 0; i < ButtonsAdjacentToCenter.length; i++)
                {
                    const cellId = ButtonsAdjacentToCenter[i]
                    if ($("#" + cellId).text() === "X")
                    {
                        adjacentId = cellId;
                        break;
                    }
                }

                if (adjacentId.length && cornerId.length)
                {
                    if (adjacentId === "cell21" && cornerId === "cell02") return $("#cell22").click();
                    if (adjacentId === "cell10" && cornerId === "cell22") return $("#cell20").click();
                    if (adjacentId === "cell01" && cornerId === "cell20") return $("#cell00").click();
                    if (adjacentId === "cell12" && cornerId === "cell00") return $("#cell02").click();

                    if (adjacentId === "cell12" && cornerId === "cell20") return $("#cell22").click();
                    if (adjacentId === "cell21" && cornerId === "cell00") return $("#cell20").click();
                    if (adjacentId === "cell10" && cornerId === "cell02") return $("#cell00").click();
                    if (adjacentId === "cell01" && cornerId === "cell22") return $("#cell02").click();
                }
            }
            
            // trzeci ruch krzyżyka - jeśli kółko jest w narożniku i przyległej 
            if (opponentSymbol === "X" && turn === 4 && $cell11.text() === "X")
            {
                let adjacentId = "";
                let cornerId = "";

                for (let i = 0; i < CornerBtns.length; i++)
                {
                    const cellId = CornerBtns[i]
                    if ($("#" + cellId).text() === "O")
                    {
                        cornerId = cellId;
                        break;
                    }
                }

                for (let i = 0; i < ButtonsAdjacentToCenter.length; i++)
                {
                    const cellId = ButtonsAdjacentToCenter[i]
                    if ($("#" + cellId).text() === "O")
                    {
                        adjacentId = cellId;
                        break;
                    }
                }

                if (adjacentId.length && cornerId.length)
                {
                    if (adjacentId === "cell21" && cornerId === "cell02") return $("#cell00").click();
                    if (adjacentId === "cell10" && cornerId === "cell22") return $("#cell02").click();
                    if (adjacentId === "cell01" && cornerId === "cell20") return $("#cell22").click();
                    if (adjacentId === "cell12" && cornerId === "cell00") return $("#cell20").click();
                }
            }
        }

        // ruchy uzupełniające 3x lub blokujące 3x
        if (cpuDifficulty === Normal || cpuDifficulty === Hard || cpuDifficulty === Insane)
        {
            const cpuWinCellNumberByColumn = CheckColumnPossibilityPutThirdSymbol(opponentSymbol);
            if (cpuCheckAllowedClick)
            {
                $("#cell" + cpuWinCellNumberByColumn).click();
                return;
            }

            const cpuBlockCelNumberByColumn = CheckColumnPossibilityPutThirdSymbol(playerSymbol);
            if (cpuCheckAllowedClick)
            {
                $("#cell" + cpuBlockCelNumberByColumn).click();
                return;
            }

            const cpuWinCellNumberByRow = CheckRowPossibilityPutThirdSymbol(opponentSymbol);
            if (cpuCheckAllowedClick)
            {
                $("#cell" + cpuWinCellNumberByRow).click();
                return;
            }

            const cpuBlockCelNumberByRow = CheckRowPossibilityPutThirdSymbol(playerSymbol);
            if (cpuCheckAllowedClick)
            {
                $("#cell" + cpuBlockCelNumberByRow).click();
                return;
            }

            const cpuWinCellNumberByFirstDiagonal = CheckFirstDiagonalPossibilityPutThirdSymbol(opponentSymbol);
            if (cpuCheckAllowedClick)
            {
                $("#cell" + cpuWinCellNumberByFirstDiagonal).click();
                return;
            }

            const cpuBlockCelNumberByFirstDiagonal = CheckFirstDiagonalPossibilityPutThirdSymbol(playerSymbol);
            if (cpuCheckAllowedClick)
            {
                $("#cell" + cpuBlockCelNumberByFirstDiagonal).click();
                return;
            }

            const cpuWinCellNumberBySecondDiagonal = CheckSecondDiagonalPossibilityPutThirdSymbol(opponentSymbol);
            if (cpuCheckAllowedClick)
            {
                $("#cell" + cpuWinCellNumberBySecondDiagonal).click();
                return;
            }

            const cpuBlockCelNumberBySecondDiagonal = CheckSecondDiagonalPossibilityPutThirdSymbol(playerSymbol);
            if (cpuCheckAllowedClick)
            {
                $("#cell" + cpuBlockCelNumberBySecondDiagonal).click();
                return;
            }
        }

        // losowy ruch
        let ok = false;
        while(!ok)
        {
            row = Math.floor(Math.random() * 3);
            column = Math.floor(Math.random() * 3);
            if($("#cell" + row + column).text() === "" )
            {
                ok = true;
            }
        }

        $("#cell" + row + column).click();
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }
}

const CheckColumnPossibilityPutThirdSymbol = (symbol) =>
{
    let ret = "";
    try 
    {
        for(let checkColumn = 0; checkColumn < 3; checkColumn++)
        {
            let columnCanEndMatch = [];
            for(let checkRow = 0; checkRow < 3; checkRow++)
            {
                if($("#cell" + checkRow + checkColumn).text() !== symbol)
                {
                    const cellNumber = checkRow.toString() + checkColumn.toString();
                    columnCanEndMatch.push(cellNumber);
                }
            }

            if (columnCanEndMatch.length === 1 && $("#cell" + columnCanEndMatch[0]).text() === "")
            {
                cpuCheckAllowedClick = true;
                ret = columnCanEndMatch[0];
                break;
            }
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }

    return ret;
}

const CheckRowPossibilityPutThirdSymbol = (symbol) =>
{
    let ret = "";
    try 
    {
        for(let checkRow = 0; checkRow < 3; checkRow++)
        {
            let rowCanEndMatch = [];
            for(let checkColumn = 0; checkColumn < 3; checkColumn++)
            {
                if($("#cell" + checkRow + checkColumn).text() !== symbol)
                {
                    const cellNumber = checkRow.toString() + checkColumn.toString();
                    rowCanEndMatch.push(cellNumber);
                }
            }

            if (rowCanEndMatch.length === 1 && $("#cell" + rowCanEndMatch[0]).text() === "")
            {
                cpuCheckAllowedClick = true;
                ret = rowCanEndMatch[0];
                break;
            }
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }

    return ret;
}

const CheckFirstDiagonalPossibilityPutThirdSymbol = (symbol) =>
{
    let ret = "";
    try 
    {
        let firstDiagonalCanEndMatch = [];
        for(let i = 0; i < 3; i++)
        {
            if($("#cell" + i + i).text() !== symbol)
            {
                const cellNumber = i.toString() + i.toString();
                firstDiagonalCanEndMatch.push(cellNumber);
            }
        }

        if (firstDiagonalCanEndMatch.length === 1 && $("#cell" + firstDiagonalCanEndMatch[0]).text() === "")
        {
            cpuCheckAllowedClick = true;
            ret = firstDiagonalCanEndMatch[0];
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }

    return ret;
}

const CheckSecondDiagonalPossibilityPutThirdSymbol = (symbol) =>
{
    let ret = "";
    try 
    {
        let secondDiagonalCanEndMatch = [];

        if($("#cell02").text() !== symbol)
        {
            secondDiagonalCanEndMatch.push("02");
        }

        if($("#cell11").text() !== symbol)
        {
            secondDiagonalCanEndMatch.push("11");
        }

        if($("#cell20").text() !== symbol)
        {
            secondDiagonalCanEndMatch.push("20");
        }


        if (secondDiagonalCanEndMatch.length === 1 && $("#cell" + secondDiagonalCanEndMatch[0]).text() === "")
        {
            cpuCheckAllowedClick = true;
            ret = secondDiagonalCanEndMatch[0];
        }
    }
    catch (ex)
    {
        LogRedError(ex);
        abp.message.error("Coś poszło nie tak :<");
    }

    return ret;
};
