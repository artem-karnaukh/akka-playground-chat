angular.module('Chat').factory("Message", function ($firebaseUtils) {
    function Message(snapshot) {
        this.$id = snapshot.key();

        this.update(snapshot);
    }

    Message.prototype = {
        update: function (snapshot) {
            var oldData = angular.extend({}, this.data);

            this.data = snapshot.val();

            //this.ChatId = this.data.ChatId;
            //this.Name = this.data.Name;
            //this.LastMessage = this.data.LastMessage;
            //this.LastMessageDate = moment(this.data.LastMessageDate).fromNow();
            //this.UnReadCount = this.data.UnReadCount;

            this.Date = moment(this.data.LastMessageDate).format("h:mm a"); 
            this.Message = this.data.Message;
            this.MessageId = this.data.MessageId;
            this.UserId = this.data.UserId;
            this.UserName = this.data.UserName;


            return !angular.equals(this.data, oldData);
        },

        getDate: function () {
            return this._date;
        },

        toJSON: function () {
            return $firebaseUtils.toJSON(this.data);
        }
    };

    return Message;
});

angular.module('Chat').factory("MessageFactory", function ($firebaseArray, Message) {
    return $firebaseArray.$extend({
        $$added: function (snap) {
            return new Message(snap);
        },

        $$updated: function (snap) {
            return this.$getRecord(snap.key()).update(snap);
        }
    });
});
