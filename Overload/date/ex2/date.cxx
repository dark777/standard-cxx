#include <iostream>

class date 
{  
   unsigned int _dia , _mes , _ano;
   std::string _sep;
   
   public:  

   date( unsigned int d, unsigned int m, unsigned int a, std::string sep=(".") ):
   _dia(d), _mes(m), _ano(a), _sep(sep)
   {
     time_t now = time(0);
     tm *ltm = localtime(&now);
     
     _dia = ltm->tm_mday;
     _mes = ltm->tm_mon+1;
     _ano = ltm->tm_year+1900;
   }
   
   std::string day()
   {
    std::string diaString = "", diatmp = "";
    diatmp = std::to_string(_dia);
    if(diatmp.length() == 1)
    diatmp.insert(0, "0");
    diaString += diatmp;
    return diaString;
   }

   std::string month()
   {
    std::string monthString = "", mestmp = "";
    mestmp = std::to_string(_mes);
    if(mestmp.length() == 1)
    mestmp.insert(0, "0"); 
    monthString += mestmp;
    return monthString;
   }

   std::string year()
   {
    std::string yearString = "", anotmp = "";
    anotmp = std::to_string(_ano);
    if(anotmp.length() == 1)
    anotmp.insert(0, "0");
    yearString += anotmp;
    return yearString; 
   }
   
   std::string datetime()
   {
    time_t _tm = time(NULL);
    
     struct tm *curtime = localtime( &_tm );
    
    return asctime(curtime);
   }
   
   friend std::ostream& operator<<( std::ostream& os , const date& dt ) 
   {
    std::string diaString = "", diatmp = "";
    diatmp = std::to_string(dt._dia);
    if(diatmp.length() == 1)
    diatmp.insert(0, "0");
    diaString += diatmp;

    std::string monthString = "", mestmp = "";
    mestmp = std::to_string(dt._mes);
    if(mestmp.length() == 1)
    mestmp.insert(0, "0"); 
    monthString += mestmp;
     
    std::string yearString = "", anotmp = "";
    anotmp = std::to_string(dt._ano);
    if(anotmp.length() == 1)
    anotmp.insert(0, "0");
    yearString += anotmp;
       
    os << diaString.c_str() << dt._sep << monthString.c_str()
       << dt._sep << yearString.c_str() << "\n\n";
       
    return os ;
   }
};

int main(void) 
{ 
  time_t mt = time(0);
  tm* tms = localtime(&mt);
  
  unsigned int dia = tms->tm_mday;
  unsigned int mes = tms->tm_mon+1;
  unsigned int ano = tms->tm_year+1900;
    
   
  date sepdate( dia, mes, ano , "-" );
  std::cout << "\n\tThe date sep "
            << sepdate << std::flush;
   
  date dt( dia, mes, ano);
  std::cout << "\n\tThe date default "
            << dt << std::flush;
    
  std::cout<<"\n\tDay......: "<<dt.day()
           <<"\n\tMonth....: "<<dt.month()     
           <<"\n\tYear.....: "<<dt.year()
           <<"\n\tDateTime.: "<<dt.datetime()
           <<"\n\n";
   return 0;     
}