#include <regex>
#include <iostream>

#define this (*this)

struct name
{
   name(std::string nome): _nome(nome){}
   
   ~name()
   {
    if(_nome.length() != 0)_nome.clear();    
   }
   
   bool isName()
   {
    std::smatch name_smatch;
    const std::regex pattern(
                             "((?:[á-úA-Za-z]+[ ]+[á-úA-Za-z]{0,20})?)?" //valida nome e sobrenome
                             "((?:[ ]+[á-úA-Za-z]{0,10})?)?" // valida nome,mais um nome composto e sobrenome
                             "((?:[ ]+[á-úA-Za-z]{0,10})?)?" // valida nome, mais dois nomes compostos e sobrenome                      
                             "((?:[ ]+[á-úA-Za-z]{0,10})?)" // valida nome, mais tres comes compostos e sobrenome
                            );
    return std::regex_match(this._nome, name_smatch, pattern);
   }
   
   name* print()
   {
    std::cout<<"\n\tNome: "<<this._nome<<(name(this._nome).isName()?"\n":" is Invalid\n");
   }
   
   private:
    std::string _nome;
};


int main()
{    
 std::string strname;
    
 do{
    
    std::cout << "Enter full name: ";
    getline(std::cin, strname);
    
    name(strname).print();
    std::cout<<"\n";
    
   }while(name(strname).isName() == 0);
  
  return 0;
}