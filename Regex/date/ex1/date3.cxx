#include <regex>
#include <iostream>




















int main()
{
 constexpr char text[]{"29/feb/2020"};
 
 std::regex re(
               R"((?:(?:(0?[1-9]|1\d|2[0-8])([-/.])(0?[1-9]|1[0-2]|j(?:an|u[nl])|ma[ry]|a(?:pr|ug)|sep|oct|nov|dec|feb)|(29|30)([-/.])(0?[13-9]|1[0-2]|j(?:an|u[nl])|ma[ry]|a(?:pr|ug)|sep|oct|nov|dec)|(31)([-/.])(0?[13578]|1[02]|jan|ma[ry]|jul|aug|oct|dec))(?:\2|\5|\8)(0{2,3}[1-9]|0{1,2}[1-9]\d|0?[1-9]\d{2}|[1-9]\d{3})|(29)([-/.])(0?2|feb)\12(\d{1,2}(?:0[48]|[2468][048]|[13579][26])|(?:0?[48]|[13579][26]|[2468][048])00)))",
               std::regex_constants::icase
              );
 
 std::cmatch match;

 if(std::regex_match(text, match, re) == 0)std::cout << "\n\tData inválida!!\n";
 else
  {
   std::cout << "\n\tDate: " << match[0]
             << "\n\tDia: " << match[1]  << match[4]  << match[7]  << match[11]
             << "\n\tMês: " << match[3]  << match[6]  << match[9]  << match[13]
             << "\n\tAno: " << match[10] << match[14] 
             << "\n\n";
  }
 return 0;
}