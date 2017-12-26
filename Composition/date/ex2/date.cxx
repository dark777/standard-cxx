#include <iostream>

struct Date
{
 unsigned int dia;
 unsigned int mes;
 unsigned int ano;
 
   Date(unsigned int, unsigned int, unsigned int);
   
   Date();
   ~Date();
   
   Date &setDia(unsigned int ,std::string);
   Date &setMes(unsigned int ,std::string);
   Date &setAno(unsigned int ,std::string);

   Date &setDia(unsigned int);
   Date &setMes(unsigned int);
   Date &setAno(unsigned int);
   
   Date &setDia();
   Date &setMes();
   Date &setAno();
   
   std::string getDia();
   std::string getMes();
   std::string getAno();
 
   friend std::ostream& operator<<(std::ostream& os, const Date& dt);
};

std::ostream& operator<<(std::ostream& os, const Date& dt)  
{
  std::string diaString = "", diatmp = "";
  diatmp = std::to_string(dt.dia);
  if (diatmp.length() == 1)
  diatmp.insert(0, "0");
  diaString += diatmp;
  
  std::string monthString = "", mestmp = "";
  mestmp = std::to_string(dt.mes);
  if(mestmp.length() == 1)
  mestmp.insert(0, "0"); 
  monthString += mestmp;
     
  std::string yearString = "", anotmp = "";
  anotmp = std::to_string(dt.ano);
  if(anotmp.length() == 1)
  anotmp.insert(0, "0");
  yearString += anotmp;
    
  os << diaString.c_str() << "/" << monthString.c_str() << "/" << yearString.c_str() << "\n";  
  return os;  
}


Date::Date(unsigned int d, unsigned int m, unsigned int a): dia(d), mes(m), ano(a){}

Date::Date(){}
Date::~Date(){}

Date &Date::setDia(unsigned int d, std::string sep)
{
  this->dia = d;
  std::string diaString = "", tmp = "";

  tmp = std::to_string(dia);
  if(tmp.length() == 1)
  tmp.insert(0, "0");
  diaString += tmp+sep;
  
  std::cout<<"\n\t"<<diaString.c_str();
  return *this;
}

Date &Date::setMes(unsigned int m, std::string sep)
{
  this->mes = m; 
  std::string monthString = "", tmp = "";

  tmp = std::to_string(mes);
  if(tmp.length() == 1)
  tmp.insert(0, "0"); 
  monthString += tmp+sep;
  
  std::cout<<monthString.c_str();
  return *this;
}

Date &Date::setAno(unsigned int a, std::string sep)
{
  this->ano = a; 
  std::string yearString = "", tmp = "";

  tmp = std::to_string(ano);
  if(tmp.length() == 1)
  tmp.insert(0, "0");
  yearString += tmp+sep;
 
  std::cout<<yearString.c_str();
  return *this;
}

Date &Date::setDia(unsigned int d)
{
  this->dia = d;
  std::string diaString = "", tmp = "";

  tmp = std::to_string(dia);
  if(tmp.length() == 1)
  tmp.insert(0, "0");
  diaString += tmp;
  
  std::cout<<"\n\t"<<diaString.c_str()<<".";
  return *this;
}

Date &Date::setMes(unsigned int m)
{
  this->mes = m;
  std::string monthString = "", tmp = "";

  tmp = std::to_string(mes);
  if(tmp.length() == 1)
  tmp.insert(0, "0"); 
  monthString += tmp;
  
  std::cout<<monthString.c_str()<<".";
  return *this;
}

Date &Date::setAno(unsigned int a)
{
  this->ano = a;
  std::string yearString = "", tmp = "";
  
  tmp = std::to_string(ano);
  if(tmp.length() == 1)
  tmp.insert(0, "0");
  yearString += tmp;
  
  std::cout<<yearString.c_str()<<"\n";
  return *this;
}

Date &Date::setDia()
{
  std::string diaString = "", tmp = "";

  tmp = std::to_string(dia);
  if(tmp.length() == 1)
  tmp.insert(0, "0");
  diaString += tmp;
  
  std::cout<<"\n\t"<<diaString.c_str()<<".";
  return *this;
}

Date &Date::setMes()
{
  std::string monthString = "", tmp = "";

  tmp = std::to_string(mes);
  if(tmp.length() == 1)
  tmp.insert(0, "0"); 
  monthString += tmp;
  
  std::cout<<monthString.c_str()<<".";
  return *this;
}

Date &Date::setAno()
{
  std::string yearString = "", tmp = "";
  
  tmp = std::to_string(ano);
  if(tmp.length() == 1)
  tmp.insert(0, "0");
  yearString += tmp;
  
  std::cout<<yearString.c_str()<<"\n";
  return *this;
}

std::string Date::getDia()
{
  std::string diaString = "", tmp = "";

  tmp = std::to_string(dia);
  if(tmp.length() == 1)
  tmp.insert(0, "0");
  diaString += tmp;
  
  return diaString.c_str();
}

std::string Date::getMes()
{
  std::string monthString = "", tmp = "";

  tmp = std::to_string(mes);
  if(tmp.length() == 1)
  tmp.insert(0, "0"); 
  monthString += tmp;
  
  return monthString.c_str();
}

std::string Date::getAno()
{
  std::string yearString = "", tmp = "";

  tmp = std::to_string(ano);
  if(tmp.length() == 1)
  tmp.insert(0, "0");
  yearString += tmp;
  
  return yearString.c_str();
}

int main()
{
  time_t mt = time(0);
  tm* tms = localtime(&mt);
  
  Date sd;
  sd.dia = tms->tm_mday;
  sd.mes = tms->tm_mon+1;
  sd.ano = tms->tm_year+1900;
  sd.setDia().setMes().setAno();
  
  sd.setDia(sd.dia,"-").setMes(sd.mes,"-").setAno(sd.ano,"\n");
  
  std::cout<<"\n\tDia: "<<sd.getDia()
           <<"\n\tMes: "<<sd.getMes()
           <<"\n\tAno: "<<sd.getAno()<<"\n\n";
  
  Date data;
  data.setDia(01).setMes(07).setAno(00);
  data.setDia(01,"-").setMes(07,"-").setAno(00,"\n");
  std::cout<<"\n\tDia: "<<data.getDia()
           <<"\n\tMes: "<<data.getMes()
           <<"\n\tAno: "<<data.getAno()<<"\n\n";
 
  
  int dia = tms->tm_mday;
  int mes = tms->tm_mon+1;
  int ano = tms->tm_year+1900;
  
  Date date;   
  date.setDia(dia,"-").setMes(mes,"-").setAno(ano,"\n");
	   
  std::cout<<"\n\t"<<Date(tms->tm_mday, tms->tm_mon+1, tms->tm_year+1900);
  
  Date d(tms->tm_mday, tms->tm_mon+1, tms->tm_year+1900);
   
  std::cout<<"\n\tDia: "<<d.getDia()
           <<"\n\tMes: "<<d.getMes()
           <<"\n\tAno: "<<d.getAno()<<"\n\n";
}