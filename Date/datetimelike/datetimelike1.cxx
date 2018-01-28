#include <regex>
#include <array>
#include <vector>
#include <chrono>
#include <cassert>
#include <utility>
#include <iostream>

enum Month
{
 Jan = 1, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec
};

// structured binding, Datastructure for string to num conversion in month(.i.e."Mar" Month to 3)
std::array<std::pair<std::string, Month>, 12> monthinfo = 
{
  std::make_pair("Jan", Month::Jan),
  std::make_pair("Feb", Month::Feb),
  std::make_pair("Mar", Month::Mar),
  std::make_pair("Apr", Month::Apr),
  std::make_pair("May", Month::May),
  std::make_pair("Jun", Month::Jun),
  std::make_pair("Jul", Month::Jul),
  std::make_pair("Aug", Month::Aug),
  std::make_pair("Sep", Month::Sep),
  std::make_pair("Oct", Month::Oct),
  std::make_pair("Nov", Month::Nov),
  std::make_pair("Dec", Month::Dec)
};

// concrete daytime structure to store the data
//template<typename T1, typename T2 = std::string>
template<class T1, class T2 = std::string>
struct DayTime
{
  T1 day   = T1();
  T1 month  = T1();
  T1 year   = T1();
  T1 hour   = T1();
  T1 min   = T1();
  T1 second  = T1();
  T2 daystr  = T2();
  T2 dtstring = T2();
};


// main class which would fetech/parse the current time and provide to the client
class CurrentDateTime
{
  DayTime<std::string> ParseDateTime(const std::string&);
  void StrToNumber(const DayTime<std::string>&);
  int GetMonth(const std::string&);
  DayTime<int> dt;

 public:

  CurrentDateTime();
  virtual ~CurrentDateTime(){};

  int GetDay()    const { return dt.day;    }
  int GetMonth()  const { return dt.month;  }
  int GetYear()   const { return dt.year;   }
  int GetHour()   const { return dt.hour;   }
  int GetMin()    const { return dt.min;    }
  int GetSecond() const { return dt.second; }
  std::string  GetDayStr() const { return dt.daystr; }
};

CurrentDateTime::CurrentDateTime()
{
  //fetch/store current local-daytime information
  auto tp = std::chrono::system_clock::now();
  time_t cstyle_t = std::chrono::system_clock::to_time_t(tp);
  char* cstyleinfo = std::ctime(&cstyle_t);
  // copy(deep) the data into the std::string as ::ctime() provides static data 
  // which might be overwritten in case someone call it again.
  std::string currentinfo(cstyleinfo);

  //parse/store the information
  auto dtstr = ParseDateTime(currentinfo);
  StrToNumber(dtstr);
}


DayTime<std::string> CurrentDateTime::ParseDateTime(const std::string& information)
{
  DayTime<std::string> info;
  std::regex dtimeregex(R"(^(\w{3}) (\w{3}) (\d{2}) (\d{2}):(\d{2}):(\d{2}) (\d{4}))");
  std::smatch match;

  if (std::regex_search(information, match, dtimeregex)) 
  {
    // Match the group and subgroups by regex parser.
    auto index    = 0;
    info.dtstring = match[index++];
    info.daystr   = match[index++];
    info.month    = match[index++];
    info.day      = match[index++];
    info.hour     = match[index++];
    info.min      = match[index++];
    info.second   = match[index++];
    info.year     = match[index++];
  }
 return info;
}

int CurrentDateTime::GetMonth(const std::string& input)
{
  for(const auto& itr : monthinfo)
   if(itr.first == input)return static_cast<int>(itr.second);
    assert(false && "Invalid month name");
  // Or return a default month if it makes sense.
  // Another possibility is extending the enum Month to 
  // add an invalid dummy value, throwing an exception, etc.
}

void CurrentDateTime::StrToNumber(const DayTime<std::string>& information)
{
  dt.dtstring = information.dtstring;
  dt.daystr   = information.daystr;
  dt.month    = GetMonth(information.month);

  dt.day    = std::stoi(information.day.c_str());
  dt.hour   = std::stoi(information.hour.c_str());
  dt.min    = std::stoi(information.min.c_str());
  dt.second = std::stoi(information.second.c_str());
  dt.year   = std::stoi(information.year.c_str());
}

int main()
{
 CurrentDateTime current = *new CurrentDateTime();

  std::cout << "\n\tCurrent Day....: " << current.GetDayStr()
            << "\n\tCurrent Date...: " << current.GetDay()
            << "\n\tCurrent Month..: " << current.GetMonth()
            << "\n\tCurrent Year...: " << current.GetYear()
            << "\n\tCurrent Hour...: " << current.GetHour()
            << "\n\tCurrent Min....: " << current.GetMin()
            << "\n\tCurrent Second.: " << current.GetSecond() 
            << "\n\n";
  return 0;
}