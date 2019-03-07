using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Tanka.GraphQL.Sample.Chat.Client.Shared.Models;

namespace Tanka.GraphQL.Sample.Chat.Client.Shared.ViewModels
{
    public class MessageViewModel : BindableBase
    {
        private readonly Message _message;

        public MessageViewModel(Message message)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Content => _message.Content;
    }
}
