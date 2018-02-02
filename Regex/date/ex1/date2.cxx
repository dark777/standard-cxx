#include <regex>
#include <iostream>






















int main()
{
 constexpr char text[]{"29.feb-2020"};
 //O R antes do abre aspas faz com que nao precise de \\ em \d       
 std::regex re(R"((?:(?:(0?[1-9]|1\d|2[0-8])[\\-|\\/|\\.](0?[1-9]|1[0-2]|j(?:an|u[nl])|ma[ry]|a(?:pr|ug)|sep|oct|nov|dec|feb)|(29|30)[\\-|\\/|\\.](0?[13-9]|1[0-2]|j(?:an|u[nl])|ma[ry]|a(?:pr|ug)|sep|oct|nov|dec)|(31)[\\-|\\/|\\.](0?[13578]|1[02]|jan|ma[ry]|jul|aug|oct|dec))[\\-|\\/|\\.](0{2,3}[1-9]|0{1,2}[1-9]\d|0?[1-9]\d{2}|[1-9]\d{3})|(29)[\\-|\\/|\\.](0?2|feb)[\\-|\\/|\\.](\d{1,2}(?:0[48]|[2468][048]|[13579][26])|(?:0?[48]|[13579][26]|[2468][048])00)))");
        
 std::cmatch match;
        
 bool valid = std::regex_match(text, match, re);
 
 if(!valid)std::cout << "\n\tData inválida!!\n\n";
 else
 {
  std::cout << "\n\tData válida: " << match[0]
            << "\n\tDia: " << match[1] << match[3] << match[5] << match[8]
            << "\n\tMês: " << match[2] << match[4] << match[6] << match[9]
            << "\n\tAno: " << match[7] << match[10]
            << "\n\n";
 }
 return 0;
}