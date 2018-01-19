#include <vector>
#include <iostream>

class mamifero 
{
  public: //Class is private by default then it is necessary to use public so that other classes or structs have access to the members
    mamifero(){}
    ~mamifero(){}
    virtual mamifero *reproduz() = 0; // Pure virtual constructor (and abstract, to enforce reimplementation in each derived class).
    virtual std::string nome() = 0;
    virtual std::string som() = 0;
};

//Class is private by default so it is necessary to use public
class cachorro: public mamifero 
{
  public:
    cachorro(){}
    ~cachorro(){}
    cachorro *reproduz(){ return new cachorro; }
    std::string nome(){ return "cachorro"; }
    std::string som() { return "latido"; }
};

class gato: public mamifero 
{
  public:
    gato(){}
    ~gato(){}
    gato *reproduz() { return new gato; }
    std::string nome(){ return "gato"; }
    std::string som() { return "miado"; }
};

class homem: public mamifero 
{
  public:
    homem(){}
    ~homem(){}
    homem *reproduz() { return new homem; }
    std::string nome(){ return "homem"; }
    std::string som() { return "fala"; }
};

class cavalo: public mamifero 
{
  public:
    cavalo(){}
    ~cavalo(){}
    cavalo *reproduz() { return new cavalo; }
    std::string nome(){ return "cavalo"; }
    std::string som() { return "relincho"; }
};

class cabra: public mamifero 
{
  public:
    cabra(){}
    ~cabra(){}
    cabra *reproduz() { return new cabra; }
    std::string nome(){ return "cabra"; }
    std::string som() { return "berro"; }
};

class leao: public mamifero 
{
  public:
    leao(){}
    ~leao(){}
    leao *reproduz() { return new leao; }
    std::string nome(){ return "leao"; }
    std::string som() { return "rugido"; }
};

class boi: public mamifero 
{
  public:
    boi(){}
    ~boi(){}
    boi *reproduz() { return new boi; }
    std::string nome(){ return "boi"; }
    std::string som() { return "sturro"; }
};

std::vector<mamifero *> opcoes
{
  new cachorro,  
  new gato,  
  new homem,
  new cavalo,
  new cabra,
  new leao,
  new boi
};

std::vector<mamifero *> ecossistema;

mamifero *menu(void)
{
  char op;
  int n=0;
  std::cout << "\n\tDiga qual animal vai se reproduzir: ";
  for(const auto &animal: opcoes)
  std::cout << "\n\t" << n++ << ": " << animal->nome() << "";
  std::cout << "\n\t--> ";
  std::cin >> n;
  std::cin.ignore(1, '\n');
  if (n >= 0 && n < opcoes.size())return opcoes[n]->reproduz();
  else
  {
   std::cout<<"\n\tEcosistema não encontrado.\n\tDeseja continuar? (s)im ou n(ão): ";
   std::cin>>op;
   if(op == 's' || op == 'S')
   return menu();
   
   else
   std::cout << "\n\tGoodbye!\n\n";
  }
  return 0;
}
constexpr unsigned int strToInt(const char* str, int h = 0)
{
 // not my code but can't remember where I got it from
 return !str[h] ? 5381 : (strToInt(str, h+1)*33) ^ str[h];
}

mamifero *getStrDetails(void)
{
  std::string resposta;

  do
  {
   std::cout << "\n\tO ecossistema tem " << ecossistema.size() << " mamífero(s).\n";
   if(ecossistema.size() > 0)
   for(const auto &animal: ecossistema)
   std::cout << "\tUm " << animal->nome() << ", que emite " << animal->som() << ".\n";   
   std::cout << "\n\tQuer inserir um novo mamífero no ecossistema?\n\tDigite [s]-Sim ou [n]-Não: ";
   
   getline(std::cin, resposta);
   std::cout << std::endl;

    switch(strToInt(resposta.c_str()))
     {
      case strToInt("s"):
      case strToInt("S"):
       ecossistema.push_back(menu());
      break;
      
      case strToInt("n"):
      case strToInt("N"):
       std::cout << "\n\tAo final, o ecossistema tinha " << ecossistema.size() << " mamífero(s).\n";
       for(const auto &animal: ecossistema)
       std::cout << "\tUm " << animal->nome() << ", que emite " << animal->som() << ".\n";
       std::cout << "\n\tGoodbye!\n\n";
       return 0;
      
      break;
      
      default:
       std::cout << "\n\tOpção Invalida!\n\n";
     }
  
  }while(1);
  return 0;
}
/*
mamifero *getDetails(void)
{
  std::string resposta;

  do
  {
   std::cout << "\n\tO ecossistema tem " << ecossistema.size() << " mamífero(s).\n";
   if(ecossistema.size() > 0)
   for(const auto &animal: ecossistema)
   std::cout << "\tUm " << animal->nome() << ", que emite " << animal->som() << ".\n";   
   std::cout << "\n\tQuer inserir um novo mamífero no ecossistema?\n\tDigite [s]-Sim ou [n]-Não: ";
   getline(std::cin, resposta);
   std::cout << std::endl;
    
   if(resposta == "s" || resposta == "S")
   ecossistema.push_back(menu());
   else
   if(resposta == "n" || resposta == "N")break;
   else
   if(resposta != "s" || resposta != "S")
   std::cout<<"\n\tOpção Inválida\n";  
   
  }while(resposta != "n" || resposta != "N");
  
  if(ecossistema.size() > 0)
   {
    std::cout << "\n\tAo final, o ecossistema tinha " << ecossistema.size() << " mamífero(s).\n";
    for(const auto &animal: ecossistema)
    std::cout << "\tUm " << animal->nome() << ", que emite " << animal->som() << ".\n";
    std::cout << "\n\tGoodbye!\n\n";
   }
  else
  std::cout << "\n\tAo final, o ecossistema tem " << ecossistema.size() << " mamífero.\n\n";
  return 0;
}
*/
int main()
{
  //getDetails();
  getStrDetails();
}