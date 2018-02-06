#ifndef _DATE_HXX
#define _DATE_HXX

#include<iostream>

struct date{

 date(int d, int m, int y): _day(d), _month(m), _year(y){}
 
 int day()   { return _day;   }
 int month() { return _month; }
 int year()  { return _year;  }
 
 date()
 {
  time_t theTime;
  tm localTime;
  
  time(&theTime);
  
  localtime_s(&localTime, &theTime);
  
  _day = localTime.tm_mday;
  _month = localTime.tm_mon;
  _year = localTime.tm_year+1900;
 }
 
 friend ostream& operator<<(ostream& os, const date &d)
 {
  return os << d._day << '/' << d._month << '/' << d._year;
 }

 private:
  int _day, _month, _year;
};

#endif 
