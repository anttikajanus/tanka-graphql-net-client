﻿using Prism.Mvvm;
using System;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Models;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.ViewModels
{
    public class MessageViewModel : BindableBase
    {
        private readonly Message _message;
        private bool _isByMe;

        public MessageViewModel(Message message)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Content => _message.Content;
        public int Id => _message.Id;
    }
}
