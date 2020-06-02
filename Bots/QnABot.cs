// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.AI.QnA.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using EchoBot.OmniChannel;
using AdaptiveCards;

namespace Microsoft.BotBuilderSamples.Bots
{

    public class QnABot<T> : ActivityHandler where T : Microsoft.Bot.Builder.Dialogs.Dialog
    {
        protected readonly BotState ConversationState;
        protected readonly Microsoft.Bot.Builder.Dialogs.Dialog Dialog;
        protected readonly BotState UserState;

        private readonly IConfiguration _configuration;

        public QnABot(ConversationState conversationState, UserState userState, T dialog, IConfiguration configuration)
        {
            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;

            _configuration = configuration;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occured during the turn.
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                /*var httpClient = new HttpClient();

                var qnaMaker = new QnAMaker(new QnAMakerEndpoint
                {
                    KnowledgeBaseId = _configuration["QnAKnowledgebaseId"],
                    EndpointKey = _configuration["QnAAuthKey"],
                    Host = GetHostname(_configuration["QnAEndpointHostName"])
                },
                null,
                httpClient);

                var options = new QnAMakerOptions { Top = 1 };

                // The actual call to the QnA Maker service.
                var response = await qnaMaker.GetAnswersAsync(turnContext, options);

                IActivity replyActivity = MessageFactory.Text($"{response[0].Answer}");

                // Replace with your own condition for bot escalation
                if (turnContext.Activity.Text.Equals("escalate", StringComparison.InvariantCultureIgnoreCase))
                {
                    Dictionary<string, object> contextVars = new Dictionary<string, object>() { { "BotHandoffTopic", "CreditCard" } };
                    OmnichannelBotClient.AddEscalationContext(replyActivity, contextVars);
                }
                // Replace with your own condition for bot end conversation
                else if (turnContext.Activity.Text.Equals("endconversation", StringComparison.InvariantCultureIgnoreCase))
                {
                    OmnichannelBotClient.AddEndConversationContext(replyActivity);
                }
                // Call method BridgeBotMessage for every response that needs to be delivered to the customer.
                else
                {
                    OmnichannelBotClient.BridgeBotMessage(replyActivity);
                }
                */

                /*
                Random r = new Random();
                var cardAttachment = CreateAdaptiveCardAttachment(_cards[r.Next(_cards.Length)]);

                //turnContext.Activity.Attachments = new List<Attachment>() { cardAttachment };
                await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
                await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
                */

                // Optional: check for any parse warnings
                // This includes things like unknown element "type"
                // or unknown properties on element
                //IList<AdaptiveWarning> warnings = result.Warnings;

                //await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);

                //await turnContext.SendActivityAsync(replyActivity, cancellationToken);

                #region Card One
                // Create a CardImage and add our image
                List<CardImage> cardImages1 = new List<CardImage>();
                cardImages1.Add(new CardImage(url: "https://www.howtogeek.com/wp-content/uploads/2018/06/shutterstock_1006988770.png"));
                // Create a CardAction to make the HeroCard clickable
                // Note this does not work in some Skype clients
                CardAction btnAiHelpWebsite = new CardAction()
                {
                    Type = "openUrl",
                    Title = "AiHelpWebsite.com",
                    Value = "http://AiHelpWebsite.com"
                };
                // Finally create the Hero Card
                // adding the image and the CardAction
                ThumbnailCard plCard1 = new ThumbnailCard()
                {
                    Title = "Ai Help Website - Number Guesser",
                    Subtitle = "Hi Welcome! - Guess a number between 1 and 5",
                    Tap = btnAiHelpWebsite
                };
                // Create an Attachment by calling the
                // ToAttachment() method of the Hero Card
                Attachment plAttachment1 = plCard1.ToAttachment();


                #endregion


                #region Card Two
                List<CardImage> cardImages2 = new List<CardImage>();
                cardImages2.Add(new CardImage(url: "https://www.wikihow.com/images/thumb/d/db/Get-the-URL-for-Pictures-Step-2-Version-6.jpg/aid597183-v4-728px-Get-the-URL-for-Pictures-Step-2-Version-6.jpg.webp"));
                // CardAction to make the HeroCard clickable
                CardAction btnTutorial = new CardAction()
                {
                    Type = "openUrl",
                    Title = "http://bit.ly/2bRyJMj",
                    Value = "http://bit.ly/2bRyJMj"
                };
                ThumbnailCard plCard2 = new ThumbnailCard()
                {
                    Title = "Based on the Using Dialogs Tutorial",
                    Subtitle = "http://bit.ly/2bRyJMj",
                    Tap = btnTutorial
                };
                Attachment plAttachment2 = plCard2.ToAttachment();
                #endregion


                var response = MessageFactory.Attachment(plAttachment1);
                response.Attachments.Add(plAttachment2);
                response.AttachmentLayout = "carousel";
                await turnContext.SendActivityAsync(response, cancellationToken);
                await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>("DialogState"), cancellationToken);



            }

            catch (AdaptiveSerializationException ex)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(ex.ToString()), cancellationToken);
            }
        }

        /*private static Attachment CreateAdaptiveCardAttachment(string filePath)
        {
            var adaptiveCardJson = File.ReadAllText(filePath);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        } */

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    OmnichannelBotClient.BridgeBotMessage(turnContext.Activity);
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello! Welcome!"), cancellationToken);
                }
            }
        }

        private static string GetHostname(string hostname)
        {
            if (!hostname.StartsWith("https://"))
            {
                hostname = string.Concat("https://", hostname);
            }

            if (!hostname.EndsWith("/qnamaker"))
            {
                hostname = string.Concat(hostname, "/qnamaker");
            }

            return hostname;
        }
    }
}
