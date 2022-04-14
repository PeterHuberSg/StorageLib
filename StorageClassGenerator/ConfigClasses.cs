/**************************************************************************************

StorageLib.ConfigClasses
========================

Some small classes to data types for data model

Written in 2020 by Jürgpeter Huber 
Contact: https://github.com/PeterHuberSg/StorageLib

To the extent possible under law, the author(s) have dedicated all copyright and 
related and neighboring rights to this software to the public domain worldwide under
the Creative Commons 0 license (details see COPYING.txt file, see also
<http://creativecommons.org/publicdomain/zero/1.0/>). 

This software is distributed without any warranty. 
**************************************************************************************/
using System;


namespace StorageLib {


  /// <summary>
  /// Stores only dates, but no time
  /// </summary>
  public class Date: IComparable<Date> {
    public int CompareTo(Date? other) {
      //Date will be replaced by DateTime, which supports IComparable.
      throw new NotImplementedException();
    }
  }


  /// <summary>
  /// Stores only times shorter than 24 hours and only with seconds precision
  /// </summary>
  public class Time: IComparable<Time> {
    public int CompareTo(Time? other) {
      //Time will be replaced by DateTime, which supports IComparable.
      throw new NotImplementedException();
    }
  }


  /// <summary>
  /// Stores dates and time with a precision of minutes
  /// </summary>
  public class DateMinutes: IComparable<DateMinutes> {
    public int CompareTo(DateMinutes? other) {
      //DateMinutes will be replaced by DateTime, which supports IComparable.
      throw new NotImplementedException();
    }
  }


  /// <summary>
  /// Stores dates and time with a precision of seconds
  /// </summary>
  public class DateSeconds: IComparable<DateSeconds> {
    public int CompareTo(DateSeconds? other) {
      //DateSeconds will be replaced by DateTime, which supports IComparable.
      throw new NotImplementedException();
    }
  }


  /// <summary>
  /// Stores dates and time with a precision of ticks
  /// </summary>
  public class DateTimeTicks: IComparable<DateTimeTicks> {
    public int CompareTo(DateTimeTicks? other) {
      //DateTimeTicks will be replaced by DateTime, which supports IComparable.
      throw new NotImplementedException();
    }
  }


  /// <summary>
  /// Stores TimeSpan with a precision of ticks
  /// </summary>
  public class TimeSpanTicks: IComparable<TimeSpanTicks> {
    public int CompareTo(TimeSpanTicks? other) {
      //TimeSpanTicks will be replaced by DateTime, which supports IComparable.
      throw new NotImplementedException();
    }
  }


  /// <summary>
  /// Stores only 2 digits after comma
  /// </summary>
  public class Decimal2: IComparable<Decimal2> {
    public int CompareTo(Decimal2? other) {
      //Decimal2 will be replaced by Decimal, which supports IComparable.
      throw new NotImplementedException();
    }
  }


  /// <summary>
  /// Stores only 4 digits after comma
  /// </summary>
  public class Decimal4: IComparable<Decimal4> {
    public int CompareTo(Decimal4? other) {
      //Decimal4 will be replaced by Decimal, which supports IComparable.
      throw new NotImplementedException();
    }
  }


  /// <summary>
  /// Stores only 5 digits after comma
  /// </summary>
  public class Decimal5: IComparable<Decimal5> {
    public int CompareTo(Decimal5? other) {
      //Decimal5 will be replaced by Decimal, which supports IComparable.
      throw new NotImplementedException();
    }
  }
}
