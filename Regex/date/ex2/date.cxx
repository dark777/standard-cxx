#include <regex>
#include <iostream>

#define this (*this)

struct date
{
 date(std::string date): _date(date){}
 
 ~date()
  {
   if(_date.length() != 0) _date.clear();
  }
 
 bool isDate()
 {
  std::smatch date_smatch;
  
  /*//regex valida datas e anos bisextos nos formatos yyyy/[A|a]/dd ou yyyy/m/dd | yyyy.[A|a].dd ou yyyy.m.dd | yyyy-[A|a]-dd ou yyyy-m-dd
  const std::regex pattern(
                           "^(?:\\d{4}[-|/|.](?:(?:(?:(?:0[13578]|1[02]|(?:[J|j]an|[M|m]ar|[M|m]ay|[J|j]ul|[A|a]ug|[O|o]ct|"
                           "[D|d]ec))[-|/|.](?:0[1-9]|[1-2][0-9]|3[01]))|(?:(?:0[469]|11|(?:[A|a]pr|[J|j]un|[S|s]ep|[N|n]ov))[-|/|.]"
                           "(?:0[1-9]|[1-2][0-9]|30))|(?:(?:02|[F|f]eb)[-|/|.](?:0[1-9]|1[0-9]|2[0-8]))))|(?:(?:\\d{2}(?:0[48]|[2468][048]|"
                           "[13579][26]))|(?:(?:[02468][048])|[13579][26])00)[-|/|.](?:02|[F|f]eb)[-|/|.]29)$"
                          );

  
  */
  
   ///regex valida datas e anos bisextos nos formatos dd/mm/yyyy ou dd/[A|a]/yyyy | dd.mm.yyyy ou dd.[A|a].yyyy | dd-mm-yyyy ou dd-[A|a]-yyyy
  const std::regex pattern(
                           "^(?:(?:0[1-9]|1[0-9]|2[0-8])[-|/|.](?:0[1-9]|1[0-2]|(?:[J|j]an|[F|f]eb|[M|m]ar|[A|a]pr|[M|m]ay|[J|j]un|[J|j]ul|[A|a]ug|[S|s]ep|[O|o]ct|[N|n]ov|[D|d]ec))|"
                           "(?:(?:29|30)[-|/|.](?:0[13456789]|1[0-2]|(?:[J|j]an|[M|m]ar|[A|a]pr|[M|m]ay|[J|j]un|[J|j]ul|[S|s]ep|[O|o]ct|[N|n]ov|[D|d]ec)))|"
                           "(?:31[-|/|.](?:0[13578]|1[02]|(?:[J|j]an|[M|m]ar|[M|m]ay|[J|j]ul|[A|a]ug|[O|o]ct|[D|d]ec))))[-|/|.](?:[2-9][0-9]{3}|1[6-9][0-9]{2}|159[0-9]|158[3-9])|"
                           "29[-|/|.](?:02|[F|f]eb)[-|/|.](?:(?:[2-9](?:04|08|[2468][048]|[13579][26])|1[6-9](?:(?:04|08|[2468][048]|[13579][26])00)|159(?:2|6)|158(?:4|8))|(?:16|[2468][048]|[3579][26])00)$"
                          );
  
  /*//Regex valida anos bisextos e nao bisextos no formato dd/mm/yyyy ou dd/str/yyyy ou dd/Str/yyyy
   const std::regex pattern(R"((?:(?:(0?[1-9]|1\d|2[0-8])[.|/|-](0?[1-9]|1[0-2]|(?:[Jj](?:an|u[nl]))|[Mm]a[ry]|(?:[Aa](?:pr|ug))|[Ss]ep|[Oo]ct|[Nn]ov|[Dd]ec|[Ff]eb)|(29|30)[.|/|-](0?[13-9]|1[0-2]|(?:[Jj](?:an|u[nl]))|[Mm]a[ry]|(?:[Aa](?:pr|ug))|[Ss]ep|[Oo]ct|[Nn]ov|[Dd]ec)|(31)[.|/|-](0?[13578]|1[02]|[Jj]an|[Mm]a[ry]|[Jj]ul|[Aa]ug|[Oo]ct|[Dd]ec))[.|/|-](0{2,3}[1-9]|0{1,2}[1-9]\d|0?[1-9]\d{2}|[1-9]\d{3})|(29)[.|/|-](0?2|[Ff]eb)[.|/|-](\d{1,2}(?:0[48]|[2468][048]|[13579][26])|(?:0?[48]|[13579][26]|[2468][048])00)))");
   */                  
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