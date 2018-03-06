#include <regex> //count
#include <iostream>

int checkEmail(std::string email)
{
 email.find("@");
 if(email.find("@")>email.length())
  return 0;
 else 
  return 1;
}

bool isMail(std::string _mail)
{
  if(count(_mail.begin(), _mail.end(), '@') != 1)
  return false;
  
  const std::string::size_type pos_at = _mail.find('@');
  
  if(pos_at == 0 || (pos_at == (_mail.length() - 1)))
  return false;
  
  if(_mail.find('.', pos_at) == std::string::npos)
  return false;
  else
  return true;
}

bool ValidarEmail(std::string email)
{
 bool validEmail = false;
 
 int indexArr = email.find('@');
 
 if(indexArr > 0)
  {
   int indexDot = email.find('.', indexArr);
   
   if(indexDot - 1 > indexArr)
    {
     if(indexDot + 1 < email.length())
      {
       std::string indexDot2 = email.substr(indexDot + 1, 1);
       if(indexDot2 != ".")validEmail = true;
      }
    }
  }
 return validEmail;
}

bool validaEmail(std::string email)
{
 std::string servidor("@gmail.com");
 
 std::string pedaco(email.substr(email.size()-servidor.size()));
 
 return !pedaco.compare(servidor);
}

int validMail(std::string input)
{
 size_t at = input.find('@');
 
 if(at == std::string::npos)
  {
   std::cout << "Missing @ symbol\n";
   return 1;
  }

 size_t dot = input.find('.', at+1);
 if(dot == std::string::npos)
  {
   std::cout << "Missing . symbol after @\n";
   return 2;
  }
  
 size_t dot2 = input.find('.', dot+1);
 if(dot2 == std::string::npos)
  {
   std::cout << "Missing . symbol after first .\n";
   return 2;
  }
 std::cout << "Email accepted.\n";
 return 0;
}

int main()
{
 std::string email;
 
 std::cout<<"\nDigite seu e-mail: ";
 std::cin>>email;
 
 validMail(email);
 /*
 if(isMail(email))
  std::cout<<"É uma conta google.\n\n";
 else 
  std::cout<<"Não é uma conta google.\n\n"; */
 return 0;
}