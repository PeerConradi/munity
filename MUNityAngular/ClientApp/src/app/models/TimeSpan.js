"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TimeSpan = /** @class */ (function () {
    function TimeSpan(milliseconds, seconds, minutes, hours, days) {
        this._timeInMilliseconds = 0;
        //
        // ### Time constants
        //
        this.msecPerSecond = 1000;
        this.msecPerMinute = 60000;
        this.msecPerHour = 3600000;
        this.msecPerDay = 86400000;
        this._timeInMilliseconds = 0;
        this._timeInMilliseconds += (days * this.msecPerDay);
        this._timeInMilliseconds += (hours * this.msecPerHour);
        this._timeInMilliseconds += (minutes * this.msecPerMinute);
        this._timeInMilliseconds += (seconds * this.msecPerSecond);
        this._timeInMilliseconds += milliseconds;
    }
    Object.defineProperty(TimeSpan.prototype, "Milliseconds", {
        get: function () { return (this._timeInMilliseconds % 1000); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TimeSpan.prototype, "Seconds", {
        get: function () { return Math.floor(((this._timeInMilliseconds / this.msecPerSecond) % 60)); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TimeSpan.prototype, "Minutes", {
        get: function () { return Math.floor(((this._timeInMilliseconds / this.msecPerMinute) % 60)); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TimeSpan.prototype, "Hours", {
        get: function () { return Math.floor(((this._timeInMilliseconds / this.msecPerHour) % 24)); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TimeSpan.prototype, "Days", {
        get: function () { return Math.floor(((this._timeInMilliseconds / this.msecPerDay))); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TimeSpan.prototype, "timeString", {
        get: function () {
            var s = '';
            if (this._timeInMilliseconds < 0) {
                s += '-';
            }
            var hour = Math.abs(this.Hours);
            var min = Math.abs(this.Minutes);
            var sec = Math.abs(this.Seconds);
            if (hour < 10)
                s += '0';
            s += hour.toString() + ':';
            if (min < 10)
                s += '0';
            s += min.toString() + ':';
            if (sec < 10)
                s += '0';
            s += sec.toString();
            return s;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TimeSpan.prototype, "TotalMilliseconds", {
        get: function () { return Math.round(this._timeInMilliseconds); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TimeSpan.prototype, "TotalSeconds", {
        get: function () { return Math.round(this._timeInMilliseconds / this.msecPerSecond); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TimeSpan.prototype, "TotalMinutes", {
        get: function () { return Math.round(this._timeInMilliseconds / this.msecPerMinute); },
        enumerable: true,
        configurable: true
    });
    TimeSpan.prototype.fromString = function (s) {
        var secString = s.split(':')[2];
        var minString = s.split(':')[1];
        var hourString = s.split(':')[0];
        if (secString != null && minString != null && hourString != null) {
            var secs = +secString;
            var mins = +minString;
            var hours = +hourString;
            if (secs != null && mins != null && hours != null) {
                this._timeInMilliseconds = 0;
                this.addSeconds(secs);
                this.addMinutes(mins);
                this.addHours(hours);
            }
        }
    };
    TimeSpan.prototype.toString = function () {
        return this.timeString;
    };
    TimeSpan.prototype.addMilliseconds = function (milliseconds) {
        this._timeInMilliseconds += milliseconds;
    };
    TimeSpan.prototype.addSeconds = function (seconds) {
        this._timeInMilliseconds += seconds * this.msecPerSecond;
    };
    TimeSpan.prototype.addMinutes = function (minutes) {
        this._timeInMilliseconds += minutes * this.msecPerMinute;
    };
    TimeSpan.prototype.addHours = function (hours) {
        this._timeInMilliseconds += hours * this.msecPerHour;
    };
    TimeSpan.prototype.addDays = function (days) {
        this._timeInMilliseconds += days * this.msecPerDay;
    };
    TimeSpan.prototype.reset = function () {
        this._timeInMilliseconds = 0;
    };
    return TimeSpan;
}());
exports.TimeSpan = TimeSpan;
//# sourceMappingURL=TimeSpan.js.map