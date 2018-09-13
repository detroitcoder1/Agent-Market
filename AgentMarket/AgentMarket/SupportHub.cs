using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace AgentMarket
{
    [Authorize]
    public class SupportHub : Hub
    {
        public void Send(string message)
        {
            string databasePath = HttpContext.Current.Server.MapPath(@"~\ObjectDatabase\support.db4o");
            string userID = HttpContext.Current.User.Identity.GetUserId();
            bool isSupport = HttpContext.Current.User.IsInRole("Support");
            using (IObjectContainer container = Db4oEmbedded.OpenFile(databasePath))
            {
                string connectionId = null;

                List<Conversation> testList = (from Conversation c in container select c).ToList();

                Conversation tempConv = null;

                if (isSupport)
                {
                    var temp = from Conversation c in container where c.SupportUserID == userID && c.FinishDate == null select c;
                    if (temp.Count() > 0)
                        connectionId = (tempConv = temp.First()).HubUserID;
                }
                else
                {
                    var temp = from Conversation c in container where c.UserID == userID && c.FinishDate == null && c.SupportUserID != null select c;
                    if (temp.Count() > 0)
                        connectionId = (tempConv = temp.First()).HubSupportUserID;
                }
                if (connectionId != null)
                {
                    Clients.Client(connectionId).addMessage(HttpContext.Current.User.Identity.GetUserName(), message);
                    Clients.Client(Context.ConnectionId).addMessage(HttpContext.Current.User.Identity.GetUserName(), message);
                    if (tempConv.Messages == null)
                        tempConv.Messages = new List<ChatMessage>();
                    tempConv.Messages.Add(new ChatMessage { PostDate = DateTime.Now, Text = message });
                    container.Store(tempConv);
                }
            }
        }

        public void Close()
        {
            string databasePath = HttpContext.Current.Server.MapPath(@"~\ObjectDatabase\support.db4o");
            string userID = HttpContext.Current.User.Identity.GetUserId();
            bool isSupport = HttpContext.Current.User.IsInRole("Support");
            if (isSupport)
                using (IObjectContainer container = Db4oEmbedded.OpenFile(databasePath))
                {
                    var conv = from Conversation c in container where c.SupportUserID == userID && c.FinishDate == null select c;
                    foreach (Conversation c in conv)
                    {
                        c.FinishDate = DateTime.Now;
                        container.Store(c);
                        Clients.Client(c.HubUserID).Notify("The matter is now considered resolved.");
                        Clients.Client(Context.ConnectionId).Notify("The matter is now considered resolved.");
                    }
                }
        }

        public void Check()
        {
            string databasePath = HttpContext.Current.Server.MapPath(@"~\ObjectDatabase\support.db4o");
            string userID = HttpContext.Current.User.Identity.GetUserId();
            bool isSupport = HttpContext.Current.User.IsInRole("Support");
            if (isSupport)
                using (IObjectContainer container = Db4oEmbedded.OpenFile(databasePath))
                {
                    var conversations = from Conversation c in container where c.SupportUserID == null select c;
                    if (conversations.Count() > 0)
                    {
                        Conversation c = conversations.First();
                        c.SupportUserID = userID;
                        c.HubSupportUserID = Context.ConnectionId;
                        c.StartDate = DateTime.Now;
                        container.Store(c);
                        Clients.Client(c.HubUserID).Notify("User '" + HttpContext.Current.User.Identity.GetUserName() + "' will assist you.");
                        Clients.Client(Context.ConnectionId).Notify("You will be helping the user '" + c.UserName + "'");
                    }
                }
        }

        public void RequestSupport()
        {
            string databasePath = HttpContext.Current.Server.MapPath(@"~\ObjectDatabase\support.db4o");
            string userID = HttpContext.Current.User.Identity.GetUserId();
            bool isSupport = HttpContext.Current.User.IsInRole("Support");
            if (!isSupport)
                using (IObjectContainer container = Db4oEmbedded.OpenFile(databasePath))
                {
                    if ((from Conversation c in container where c.UserID == userID && c.FinishDate == null select c).Count() == 0)
                    {
                        string username = HttpContext.Current.User.Identity.GetUserName();
                        container.Store(new Conversation { UserID = userID, UserName = username, HubUserID = Context.ConnectionId });
                    }
                }
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            string databasePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~\ObjectDatabase\support.db4o");
            string userID = Context.User.Identity.GetUserId();
            bool isSupport = Context.User.IsInRole("Support");
            using (IObjectContainer container = Db4oEmbedded.OpenFile(databasePath))
            {
                IEnumerable<ChatUser> users = from ChatUser user in container where user.UserID == userID select user;
                if (users.Count() == 0)
                    container.Store(new ChatUser { UserID = userID, IsSupport = isSupport, HubUserID = Context.ConnectionId });
                else
                {
                    ChatUser temp = users.First();
                    temp.HubUserID = Context.ConnectionId;
                    container.Store(temp);
                }
                if (isSupport)
                {
                    var conversations = from Conversation c in container where c.SupportUserID == null select c;
                    if (conversations.Count() > 0)
                    {
                        Conversation c = conversations.First();
                        c.SupportUserID = userID;
                        c.HubSupportUserID = Context.ConnectionId;
                        c.StartDate = DateTime.Now;
                        container.Store(c);
                        Clients.Client(c.HubUserID).Notify("User '" + HttpContext.Current.User.Identity.GetUserName() + "' will assist you.");
                        Clients.Client(Context.ConnectionId).Notify("You will be helping the user '" + c.UserName + "'");
                    }
                }
            }
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            string databasePath = HttpContext.Current.Server.MapPath(@"~\ObjectDatabase\support.db4o");
            using (IObjectContainer container = Db4oEmbedded.OpenFile(databasePath))
            {
                IEnumerable<ChatUser> users = from ChatUser user in container where user.HubUserID == Context.ConnectionId select user;
                foreach (ChatUser user in users)
                    container.Delete(user);
            }
            return base.OnDisconnected();
        }
    }

    public class ChatMessage
    {
        public string Text { get; set; }
        public DateTime PostDate { get; set; }
    }

    public class Conversation
    {
        public List<ChatMessage> Messages { get; set; }
        public string HubUserID { get; set; }
        public string HubSupportUserID { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string SupportUserID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }

    public class ChatUser
    {
        public string UserID { get; set; }
        public string HubUserID { get; set; }
        public bool IsSupport { get; set; }
    }
}