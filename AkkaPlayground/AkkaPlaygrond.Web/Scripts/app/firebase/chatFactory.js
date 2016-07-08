angular.module('Chat').factory("Chat", function ($firebaseUtils) {
    function Chat(snapshot) {
        this.$id = snapshot.key();

        this.update(snapshot);
    }

    Chat.prototype = {
        update: function (snapshot) {
            var oldData = angular.extend({}, this.data);

            this.data = snapshot.val();

            this.ChatId = this.data.ChatId;
            this.Name = this.data.Name;
            this.LastMessage = this.data.LastMessage;
            this.LastMessageDate = moment(this.data.LastMessageDate).fromNow();
            this.UnReadCount = this.data.UnReadCount;

            return !angular.equals(this.data, oldData);
        },

        getDate: function () {
            return this._date;
        },

        toJSON: function () {
            return $firebaseUtils.toJSON(this.data);
        }
    };

    return Chat;
});

angular.module('Chat').factory("ChatFactory", function ($firebaseArray, Chat) {
    return $firebaseArray.$extend({
        $$added: function (snap) {
            return new Chat(snap);
        },

        $$updated: function (snap) {
            return this.$getRecord(snap.key()).update(snap);
        }
    });
});
