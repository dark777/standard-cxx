#include<iostream>

struct Date
{
 std::string day="";
 std::string month="";
 std::string year="";
 
 std::string sday="";
 std::string smonth="";
 std::string syear="";
 
   Date()
   {
    time_t now(time(nullptr));
    tm *ltm(localtime(&now));

    sday = std::to_string(ltm->tm_mday);
    if(sday.length() == 1)
    sday.insert(0,"0");
    
    smonth = std::to_string(ltm->tm_mon+1);
    if(smonth.length() == 1)
    smonth.insert(0,"0");
    
    syear = std::to_string(ltm->tm_year+1900);
    if(syear.length() == 1)
    syear.insert(0,"0");
    
    day = std::to_string(ltm->tm_mday);
    month = std::to_string(ltm->tm_mon+1);
    year = std::to_string(ltm->tm_year+1900);
   }
   
   ~Date()
    {      
     if(sday.length() != 0)sday.clear();
     if(smonth.length() != 0)smonth.clear();     
     if(smonth.length() != 0)syear.clear();
     
     if(day.length() != 0)day.clear();
     if(month.length() != 0)month.clear();
     if(year.length() != 0)year.clear();
    }
   
   std::string toInt()
   {
    return std::string(day+"/"+month+"/"+year);
   }
   
   std::string toString()
   {
    return std::string(sday+"/"+smonth+"/"+syear);
   }
   
   std::string dateTime()
   {
    strftime(buf, sizeof(buf), "%a, %d %b %Y %H:%M:%S GMT", now);
    return buf;
   }
   
  private:
  
  char buf[50];
  time_t tms = time(NULL);
  tm *now = localtime(&tms);
  
}date; 

int main()
{
 std::cout << "\n\tThe date is Int "
           << "\n\tThe day is.............: " << date.day      
           << "\n\tThe month is...........: " << date.month
           << "\n\tThe year is............: " << date.year
           << "\n\tThe date Int Format....: " << date.toInt()
           << "\n\n\tThe date is Str "
           << "\n\tdate time..............: " << date.dateTime()
           << "\n\tThe day is.............: " << date.sday      
           << "\n\tThe month is...........: " << date.smonth
           << "\n\tThe year is............: " << date.syear
           << "\n\tThe date String Format.: " << date.toString() 
           << "\n\n";
 return 0;   
}