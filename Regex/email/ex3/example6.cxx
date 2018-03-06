#include <regex>
#include <iostream>

bool is_email_valid(std::string email)
{
 // define a regular expression
 const std::regex pattern("(\\w+)(\\.|_)?(\\w*)@(\\w+)(\\.(\\w+))?(\\.(\\w+))");
 
 // try to match the string with the regular expression
 return std::regex_match(email, pattern);
}

int main()
{
 std::string email[3] = {
                         "text.example@gmail.com",
                         "text.exa_mple@yahoo.com.br",
                         "text.example@terra.com.br.net",
                        };
 for(int i=0; i<3;i++)
 std::cout << "\n\tEmail: " << email[i] << " : " << (is_email_valid(email[i]) ?" is Valid\n" : " is inValid\n");
 
 return 0;
}