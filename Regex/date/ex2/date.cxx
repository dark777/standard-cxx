#include <regex>
#include <iostream>

#define this (*this)

struct date
{
 date(std::string mail): _date(mail){}
 
 ~date()
  {
   if(_date.length() != 0) _date.clear();
  }
 
 bool isDate()
 {
  std::smatch date_smatch;
  
  //regex valida datas e anos bisextos nos formatos yyyy/[A|a]/dd ou yyyy/m/dd | yyyy.[A|a].dd ou yyyy.m.dd | yyyy-[A|a]-dd ou yyyy-m-dd
  const std::regex pattern("^(?:\\d{4}(?:/|.|-)(?:(?:(?:(?:0[13578]|1[02]|(?:jan|mar|may|jul|aug|oct|dec|Jan|Mar|May|Jul|Aug|Oct|Dec))(?:/|.|-)(?:0[1-9]|[1-2][0-9]|3[01]))|(?:(?:0[469]|11|(?:apr|jun|sep|nov|Apr|Jun|S‌​ep|Nov))(?:/|.|-)(?:0[1-9]|[1-2][0-9]|30))|(?:(?:02|feb|Feb)(?:/|.|-)(?:0[1-9]|1[0-9]|2[0-8]))))|(?:(?:\\d{2}(?:0[48]|[2468][048]|[13579][26]))|(?:(?:[02468][048])|[13579][26])00)(?:/|.|-)(?:02|feb|Feb)(?:/|.|-)29)$");
 
  return std::regex_match(this._date, date_smatch, pattern);
 }
 
 date& print()
 {
  std::cout<<"\n\tDate: "<<this._date<<(date(this._date).isDate()?" is Valid\n":" is Invalid\n");
  return this;
 }
 
 private:
  std::string _date;
};

int main()
{    
 std::string str;
    
 do{
    
    std::cout << "\n\tEnter date: ";
    getline(std::cin, str);
    
    date(str).print();
    std::cout<<"\n";
    
   }while(date(str).isDate() == 1);
  
  return 0;
}