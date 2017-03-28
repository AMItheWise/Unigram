﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Api;
using Telegram.Api.Aggregator;
using Telegram.Api.Helpers;
using Telegram.Api.Services;
using Telegram.Api.Services.Cache;
using Telegram.Api.TL;
using Unigram.Common;
using Unigram.Views;
using Unigram.Views.Login;
using Windows.UI.Xaml.Navigation;

namespace Unigram.ViewModels.Login
{
    public class LoginSignupViewModel : UnigramViewModelBase
    {
        private string _phoneNumber;
        private string _phoneCode;
        private TLAuthSentCode _sentCode;

        public LoginSignupViewModel(IMTProtoService protoService, ICacheService cacheService, ITelegramEventAggregator aggregator) 
            : base(protoService, cacheService, aggregator)
        {
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var param = parameter as LoginSignUpPage.NavigationParameters;
            if (param != null)
            {
                _phoneNumber = param.PhoneNumber;
                _phoneCode = param.PhoneCode;
                _sentCode = param.Result;
            }

            return Task.CompletedTask;
        }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                Set(ref _firstName, value);
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                Set(ref _lastName, value);
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                Set(ref _isLoading, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _sendCommand;
        public RelayCommand SendCommand => _sendCommand = _sendCommand ?? new RelayCommand(SendExecute, () => !IsLoading);
        private async void SendExecute()
        {
            if (_sentCode == null)
            {
                //...
                return;
            }

            var phoneNumber = _phoneNumber;
            var phoneCodeHash = _sentCode.PhoneCodeHash;

            IsLoading = true;

            var result = await ProtoService.SignUpAsync(phoneNumber, phoneCodeHash, _phoneCode, _firstName, _lastName);
            if (result.IsSucceeded)
            {
                ProtoService.SetInitState();
                ProtoService.CurrentUserId = result.Result.User.Id;
                SettingsHelper.IsAuthorized = true;
                SettingsHelper.UserId = result.Result.User.Id;

                // TODO: maybe ask about notifications?

                NavigationService.Navigate(typeof(MainPage));
            }
            else
            {
                IsLoading = false;

                if (result.Error.TypeEquals(TLErrorType.PHONE_NUMBER_UNOCCUPIED))
                {
                    //var signup = await ProtoService.SignUpAsync(phoneNumber, phoneCodeHash, PhoneCode, "Paolo", "Veneziani");
                    //if (signup.IsSucceeded)
                    //{
                    //    ProtoService.SetInitState();
                    //    ProtoService.CurrentUserId = signup.Value.User.Id;
                    //    SettingsHelper.IsAuthorized = true;
                    //    SettingsHelper.UserId = signup.Value.User.Id;
                    //}

                    //this._callTimer.Stop();
                    //this.StateService.ClearNavigationStack = true;
                    //this.NavigationService.UriFor<SignUpViewModel>().Navigate();
                }
                else if (result.Error.TypeEquals(TLErrorType.PHONE_CODE_INVALID))
                {
                    //await new MessageDialog(Resources.PhoneCodeInvalidString, Resources.Error).ShowAsync();
                }
                else if (result.Error.TypeEquals(TLErrorType.PHONE_CODE_EMPTY))
                {
                    //await new MessageDialog(Resources.PhoneCodeEmpty, Resources.Error).ShowAsync();
                }
                else if (result.Error.TypeEquals(TLErrorType.PHONE_CODE_EXPIRED))
                {
                    //await new MessageDialog(Resources.PhoneCodeExpiredString, Resources.Error).ShowAsync();
                }
                else if (result.Error.TypeEquals(TLErrorType.SESSION_PASSWORD_NEEDED))
                {
                    //this.IsWorking = true;
                    var password = await ProtoService.GetPasswordAsync();
                    if (password.IsSucceeded)
                    {
                        NavigationService.Navigate(typeof(LoginPasswordPage), password.Result);
                    }
                    else
                    {
                        Execute.ShowDebugMessage("account.getPassword error " + password.Error);
                    }
                }
                else if (result.Error.CodeEquals(TLErrorCode.FLOOD))
                {
                    //await new MessageDialog($"{Resources.FloodWaitString}\r\n\r\n({error.Message})", Resources.Error).ShowAsync();
                }

                Execute.ShowDebugMessage("account.signIn error " + result.Error);
            }
        }

    }
}