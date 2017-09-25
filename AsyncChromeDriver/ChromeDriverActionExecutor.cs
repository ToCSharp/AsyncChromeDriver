// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;
using Zu.AsyncWebDriver.Interactions;

namespace Zu.Chrome
{
    internal class ChromeDriverActionExecutor : IActionExecutor
    {
        private AsyncChromeDriver asyncChromeDriver;
        private CancellationTokenSource performActionsCancellationTokenSource;

        public ChromeDriverActionExecutor(AsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public Task<bool> IsActionExecutor(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }

        public async Task PerformActions(IList<ActionSequence> actionSequenceList, CancellationToken cancellationToken = default(CancellationToken))
        {
            performActionsCancellationTokenSource = new CancellationTokenSource();
            using (CancellationTokenSource linkedCts =
               CancellationTokenSource.CreateLinkedTokenSource(performActionsCancellationTokenSource.Token, cancellationToken))
            {
                try
                {
                    var ct = linkedCts.Token;
                    ct.ThrowIfCancellationRequested();
                    foreach (var action in actionSequenceList)
                    {
                        ct.ThrowIfCancellationRequested();
                        cancellationToken.ThrowIfCancellationRequested();
                        foreach (var interaction in action.Interactions)
                        {
                            //await Task.Delay(100);
                            if (interaction is PauseInteraction)
                            {
                                await Task.Delay(((PauseInteraction)interaction).Duration, ct);
                            }
                            else if (interaction is PointerInputDevice.PointerDownInteraction)
                            {
                                var pdi = (PointerInputDevice.PointerDownInteraction)interaction;
                                var pk = ((PointerInputDevice)interaction.SourceDevice).PointerKind;
                                if (pk == PointerKind.Mouse)
                                {
                                    if (pdi.Button == MouseButton.Left)
                                    {
                                        await asyncChromeDriver.Mouse.MouseDown(asyncChromeDriver.Session.mouse_position, ct);
                                    }
                                    else if (pdi.Button == MouseButton.Right)
                                    {
                                        await asyncChromeDriver.Mouse.ContextClick(asyncChromeDriver.Session.mouse_position, ct);
                                    }
                                }
                                else if (pk == PointerKind.Touch)
                                {
                                    if (pdi.Button == MouseButton.Left)
                                    {
                                        await asyncChromeDriver.TouchScreen.Down(asyncChromeDriver.Session.mouse_position.X, asyncChromeDriver.Session.mouse_position.Y, ct);
                                    }
                                    else if (pdi.Button == MouseButton.Right)
                                    {
                                        throw new NotSupportedException("Touch with MouseButton.Right");
                                    }
                                }
                                else if (pk == PointerKind.Pen)
                                {
                                    throw new NotImplementedException("PointerKind.Pen");
                                }
                            }
                            else if (interaction is PointerInputDevice.PointerUpInteraction)
                            {
                                var pui = (PointerInputDevice.PointerUpInteraction)interaction;
                                var pk = ((PointerInputDevice)interaction.SourceDevice).PointerKind;
                                if (pk == PointerKind.Mouse)
                                {
                                    if (pui.Button == MouseButton.Left)
                                    {
                                        await asyncChromeDriver.Mouse.MouseUp(asyncChromeDriver.Session.mouse_position, ct);
                                    }
                                    else if (pui.Button == MouseButton.Right)
                                    {
                                        await asyncChromeDriver.Mouse.ContextClick(asyncChromeDriver.Session.mouse_position, ct);
                                    }
                                }
                                else if (pk == PointerKind.Touch)
                                {
                                    throw new NotSupportedException("Touch with MouseButton.Right");
                                }
                                else if (pk == PointerKind.Pen)
                                {
                                    throw new NotImplementedException("PointerKind.Pen");
                                }
                            }
                            else if (interaction is PointerInputDevice.PointerCancelInteraction)
                            {

                            }
                            else if (interaction is PointerInputDevice.PointerMoveInteraction)
                            {
                                var pmi = (PointerInputDevice.PointerMoveInteraction)interaction;
                                var pk = ((PointerInputDevice)interaction.SourceDevice).PointerKind;
                                if (pk == PointerKind.Mouse)
                                {
                                    if (pmi.Target != null)
                                    {
                                        if (pmi.X != 0 || pmi.Y != 0)
                                        {
                                            WebPoint location = await pmi.Target.Location();
                                            location = location.Offset(pmi.X, pmi.Y);
                                            await asyncChromeDriver.Mouse.MouseMove(location, ct);
                                        }
                                        else
                                        {
                                            //WebPoint location = await asyncChromeDriver.ElementUtils.GetElementClickableLocation(pmi.Target.Id, ct);
                                            //if (location == null) 
                                            var location = await asyncChromeDriver.Elements.GetElementLocation(pmi.Target.Id, ct);
                                            await asyncChromeDriver.Mouse.MouseMove(location, ct);
                                        }
                                    }
                                    else await asyncChromeDriver.Mouse.MouseMove(asyncChromeDriver.Session.mouse_position.Offset(pmi.X, pmi.Y), ct);
                                }
                                else if (pk == PointerKind.Touch)
                                {
                                    if (pmi.Target != null)
                                    {
                                        if (pmi.X != 0 || pmi.Y != 0)
                                        {
                                            WebPoint location = await pmi.Target.Location();
                                            location = location.Offset(pmi.X, pmi.Y);
                                            await asyncChromeDriver.TouchScreen.Move(location.X, location.Y, ct);
                                        }
                                        else
                                        {
                                            //WebPoint location = await asyncChromeDriver.ElementUtils.GetElementClickableLocation(pmi.Target.Id);
                                            var location = await asyncChromeDriver.Elements.GetElementLocation(pmi.Target.Id, ct);
                                            if (location != null) await asyncChromeDriver.TouchScreen.Move(location.X, location.Y, ct);
                                        }
                                    }
                                    else
                                    {
                                        var newLoc = asyncChromeDriver.Session.mouse_position.Offset(pmi.X, pmi.Y);
                                        await asyncChromeDriver.TouchScreen.Move(newLoc.X, newLoc.Y, ct);
                                    }
                                }
                                else if (pk == PointerKind.Pen)
                                {
                                    throw new NotImplementedException("PointerKind.Pen");
                                }
                            }
                            else if (interaction is KeyInputDevice.KeyDownInteraction)
                            {
                                var value = ((KeyInputDevice.KeyDownInteraction)interaction).GetValue();
                                await asyncChromeDriver.Keyboard.PressKey(value, ct);
                            }
                            else if (interaction is KeyInputDevice.KeyUpInteraction)
                            {
                                var value = ((KeyInputDevice.KeyUpInteraction)interaction).GetValue();
                                await asyncChromeDriver.Keyboard.ReleaseKey(value, ct);
                            }
                        }
                    }
                }
                catch { throw; }
            }
        }

        public Task ResetInputState(CancellationToken cancellationToken = default(CancellationToken))
        {
            return CancelCurrentActions();
        }

        private Task CancelCurrentActions()
        {
            performActionsCancellationTokenSource?.Cancel();
            return Task.CompletedTask;
        }
    }
}