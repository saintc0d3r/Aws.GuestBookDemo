function Notification() {
    this.notify = function (content, duration, fadeIn, horizontal, vertical) {
        $.extend($.notificationOptions, { fadeIn: fadeIn });
        $.createNotification({
            horizontal: horizontal,
            vertical: vertical,
            content: content,
            duration: duration
        });
    };
}