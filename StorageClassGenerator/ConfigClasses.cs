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
  public class Date {}


  /// <summary>
  /// Stores only times shorter than 24 hours and only with seconds precision
  /// </summary>
  public class Time {}


  /// <summary>
  /// Stores dates and time with a precision of minutes
  /// </summary>
  public class DateMinutes { }


  /// <summary>
  /// Stores dates and time with a precision of seconds
  /// </summary>
  public class DateSeconds { }


  /// <summary>
  /// Stores dates and time with a precision of ticks
  /// </summary>
  public class DateTimeTicks { }


  /// <summary>
  /// Stores TimeSpan with a precision of ticks
  /// </summary>
  public class TimeSpanTicks { }


  /// <summary>
  /// Stores only 2 digits after comma
  /// </summary>
  public class Decimal2 { }


  /// <summary>
  /// Stores only 4 digits after comma
  /// </summary>
  public class Decimal4 { }


  /// <summary>
  /// Stores only 5 digits after comma
  /// </summary>
  public class Decimal5 { }
}
