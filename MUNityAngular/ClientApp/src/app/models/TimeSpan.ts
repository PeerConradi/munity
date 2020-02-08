export class TimeSpan {

  private _timeInMilliseconds: number = 0;

  //
  // ### Time constants
  //
  private msecPerSecond = 1000;
  private msecPerMinute = 60000;
  private msecPerHour   = 3600000;
  private msecPerDay    = 86400000;


  public get Milliseconds(): number { return (this._timeInMilliseconds % 1000); }
  public get Seconds(): number { return Math.floor(((this._timeInMilliseconds / this.msecPerSecond) % 60)); }
  public get Minutes(): number { return Math.floor(((this._timeInMilliseconds / this.msecPerMinute) % 60)); }
  public get Hours(): number { return Math.floor(((this._timeInMilliseconds / this.msecPerHour) % 24)); }
  public get Days(): number { return Math.floor(((this._timeInMilliseconds / this.msecPerDay))); }

  public get timeString(): string {
    let s = '';
    if (this._timeInMilliseconds < 0) {
      s += '-';
    }

    const hour = Math.abs(this.Hours);
    const min = Math.abs(this.Minutes);
    const sec = Math.abs(this.Seconds);

    if (hour < 10) s += '0';
    s += hour.toString() + ':';

    if (min < 10) s += '0';
    s += min.toString() + ':';

    if (sec < 10) s += '0';
    s += sec.toString();

    return s;
  }

  

  public get TotalMilliseconds(): number { return Math.round(this._timeInMilliseconds); }
  public get TotalSeconds(): number { return Math.round(this._timeInMilliseconds / this.msecPerSecond); }
  public get TotalMinutes(): number { return Math.round(this._timeInMilliseconds / this.msecPerMinute); }

  constructor (milliseconds: number, seconds: number, minutes: number, hours: number, days: number) {
    this._timeInMilliseconds = 0;
    
      this._timeInMilliseconds += (days * this.msecPerDay);
      this._timeInMilliseconds += (hours * this.msecPerHour);
      this._timeInMilliseconds += (minutes * this.msecPerMinute);
      this._timeInMilliseconds += (seconds * this.msecPerSecond);
      this._timeInMilliseconds += milliseconds;
  }

  fromString(s: string) {
    const secString = s.split(':')[2];
    const minString = s.split(':')[1];
    const hourString = s.split(':')[0];
    if (secString != null && minString != null && hourString != null) {
      const secs: number = +secString;
      const mins: number = +minString;
      const hours: number = +hourString;
      if (secs != null && mins != null && hours != null) {
        this._timeInMilliseconds = 0;
        this.addSeconds(secs);
        this.addMinutes(mins);
        this.addHours(hours);
      }
    }
  }

  toString(): string {
    return this.timeString;
  }

  addMilliseconds(milliseconds: number) {
    this._timeInMilliseconds += milliseconds;
  }

  addSeconds(seconds: number) {
    this._timeInMilliseconds += seconds * this.msecPerSecond;
  }

  addMinutes(minutes: number) {
    this._timeInMilliseconds += minutes * this.msecPerMinute;
  }

  addHours(hours: number) {
    this._timeInMilliseconds += hours * this.msecPerHour;
  }

  addDays(days: number) {
    this._timeInMilliseconds += days * this.msecPerDay;
  }

  public reset() {
    this._timeInMilliseconds = 0;
  }
}
