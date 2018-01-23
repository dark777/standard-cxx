#include "libmenu.hxx"

#define MAX 15

menu::menu(int opmenu, const char* opcoes[])
{
 if(opmenu > MAX)
 {
  std::cout<<"OPTION LIMITS MAXIMUM SIZE: "<<MAX<<" EXCEEDED OPTION UNSUPORTED: "<<opmenu<<"\n";
  exit(1);
 }
 else
 for(int i = 0; i<opmenu; i++)
 std::cout<<"\n\t["<<i+1<<"]-"<<opcoes[i];
 std::cout<<"\n\tESCOLHA: ";
}

menu::menu(int opmenu, int maxlimits, const char* opcoes[])
{
 if(opmenu != maxlimits)
 {
  std::cout<<"OPTION LIMITS: "<<opmenu<<" DOES NOT EQUAL TO MAX: "<<maxlimits<<"\n";
  exit(1);
 }
 else
 for(int i = 0; i<opmenu; i++)
 std::cout<<"\n\t["<<i+1<<"]-"<<opcoes[i];
 std::cout<<"\n\tESCOLHA: ";
}

menu::menu(int opmenu, std::string* opcoes)
{
 if(opmenu > MAX)
 {
  std::cout<<"OPTION LIMITS MAXIMUM SIZE: "<<MAX<<" EXCEEDED OPTION UNSUPORTED: "<<opmenu<<"\n";
  exit(1);
 }
 else
 for(int i = 0; i<opmenu; i++)
 std::cout<<"\n\t["<<i+1<<"]-"<<opcoes[i];
 std::cout<<"\n\tESCOLHA: ";
}

menu::menu(int opmenu, int maxlimits, std::string* opcoes)
{
 if(opmenu != maxlimits)
 {
  std::cout<<"OPTION LIMITS: "<<opmenu<<" DOES NOT EQUAL TO MAX: "<<maxlimits<<"\n";
  exit(1);
 }
 else
 for(int i = 0; i<opmenu; i++)
 std::cout<<"\n\t["<<i+1<<"]-"<<opcoes[i];
 std::cout<<"\n\tESCOLHA: ";
}