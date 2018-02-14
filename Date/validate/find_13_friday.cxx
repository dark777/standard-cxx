#include <cstdio>
#include <vector>
#include <cassert>
//https://gist.github.com/if1live/d99645fc3e8932f9a9db
enum { DAY_MON = 0,DAY_TUE,DAY_WED,DAY_THU,DAY_FRI,DAY_SAT,DAY_SUN };

template<int Y>
struct LeapYear
{
 enum { value = ((Y % 4 == 0) && (Y % 100 != 0)) || (Y % 400 == 0), total = value ? 366 : 365 };
};

static_assert(LeapYear<2004>::value == true, "");
static_assert(LeapYear<2008>::value == true, "");
static_assert(LeapYear<2012>::value == true, "");
static_assert(LeapYear<2016>::value == true, "");

static_assert(LeapYear<1900>::value == false, "");
static_assert(LeapYear<2100>::value == false, "");
static_assert(LeapYear<2200>::value == false, "");
static_assert(LeapYear<2300>::value == false, "");

static_assert(LeapYear<1600>::value == true, "");
static_assert(LeapYear<2000>::value == true, "");
static_assert(LeapYear<2400>::value == true, "");

template<int idx, int I, int... Remainder>
struct ElementAt
{
 enum { value = ElementAt<idx - 1, Remainder...>::value };
};

template<int I, int... Remainder>
struct ElementAt <0, I, Remainder...> 
{
 enum { value = I };
};

static_assert(ElementAt<0, 1, 2, 4>::value == 1, "");   //[1, 2, 4][0]
static_assert(ElementAt<1, 1, 2, 4>::value == 2, "");   //[1, 2, 4][1]
static_assert(ElementAt<2, 1, 2, 4>::value == 4, "");   //[1, 2, 4][2]

template<int Month>
struct NormalYearDayCount
{
 enum { value = ElementAt<Month, 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31>::value };
};

template<int Month>
struct LeapYearDayCount
{
 enum { value = ElementAt<Month, 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31>::value };
};

static_assert(NormalYearDayCount<2>::value == 28, "");
static_assert(LeapYearDayCount<2>::value == 29, "");

template<int Year, int Month>
struct YearDayCount
{
 enum { value = LeapYear<Year>::value ? LeapYearDayCount<Month>::value : NormalYearDayCount<Month>::value };
};

static_assert(YearDayCount<2015, 2>::value == 28, "");
static_assert(YearDayCount<2016, 2>::value == 29, "");

template<int Year, int Month, int Day>
struct DayCount
{
 enum { value = DayCount<Year, Month, 1>::value + Day - 1 };
};

template<int Year, int Month>
struct DayCount < Year, Month, 1 >
{
 enum { value = DayCount<Year, Month - 1, 1>::value + YearDayCount<Year, Month - 1>::value };
};

template<int Year>
struct DayCount <Year, 1, 1>
{
 enum { value = 0 };
};

static_assert(DayCount<2015, 1, 1>::value == 0, "");
static_assert(DayCount<2015, 1, 11>::value == 10, "");

static_assert(DayCount<2015, 2, 1>::value == 31, "");
static_assert(DayCount<2015, 3, 1>::value == 31 + 28, "");
static_assert(DayCount<2015, 4, 1>::value == 31 + 28 + 31, "");

template<int Year>
struct YearTotalDay
{
 enum { value = YearTotalDay<Year - 1>::value + LeapYear<Year - 1>::total };
};

template<>
struct YearTotalDay <2000>
{
 // 2000/01/01 = SAT
 enum { value = DAY_SAT };
};

static_assert(YearTotalDay<2000>::value == DAY_SAT, "");
static_assert(YearTotalDay<2001>::value % 7 == DAY_MON, "");
static_assert(YearTotalDay<2002>::value % 7 == DAY_TUE, "");

template<int Y, int M, int D>
struct Weekday
{
 enum { start = YearTotalDay<Y>::value % 7, month_start = (DayCount<Y, M, 1>::value + start) % 7, value = (DayCount<Y, M, D>::value + start) % 7};
};

static_assert(Weekday<2001, 1, 1>::value == DAY_MON, "");
static_assert(Weekday<2002, 1, 1>::value == DAY_TUE, "");
static_assert(Weekday<2003, 1, 1>::value == DAY_WED, "");
static_assert(Weekday<2015, 1, 1>::value == DAY_THU, "");

static_assert(Weekday<2015, 1, 1>::month_start == DAY_THU, "");
static_assert(Weekday<2015, 2, 1>::month_start == DAY_SUN, "");
static_assert(Weekday<2015, 3, 1>::month_start == DAY_SUN, "");

static_assert(Weekday<2015, 1, 13>::value == DAY_TUE, "");
static_assert(Weekday<2015, 2, 13>::value == DAY_FRI, "");
static_assert(Weekday<2015, 3, 13>::value == DAY_FRI, "");

template<int v>
struct Int2Type
{
 enum { value = v };
};

template<int Year, int Month>
struct EvilYear
{
 typedef EvilYear<Year, Month + 1> Next;

 enum { Weekday = Weekday<2015, Month, 13>::value, value = Weekday == DAY_FRI};

 static void run(std::vector<int> &result) { run(result, Int2Type<value>());}

 static void run(std::vector<int> &result, Int2Type<true>)
 {
  result.push_back(Month);
  Next::run(result);
 }

 static void run(std::vector<int> &result, Int2Type<false>)
 {
  Next::run(result);
 }
};

template<int Year>
struct EvilYear<Year, 12>
{
 enum { Month = 12, Weekday = Weekday<2015, Month, 13>::value, value = Weekday == DAY_FRI };

 static void run(std::vector<int> &result)
 {
  run(result, Int2Type<value>());
 }

 static void run(std::vector<int> &result, Int2Type<true>)
 {
  result.push_back(Month);
 }
 
 static void run(std::vector<int> &result, Int2Type<false>){}
};

template<int Year>
struct EvilYearRunner
{
 typedef EvilYear<Year, 1> Start;

 static void run(std::vector<int> &result)
 {
  Start::run(result);
 }
};

int main()
{
 const int Year = 2015;

 std::vector<int> retval;
 EvilYearRunner<Year>::run(retval);

 if(Year == 2015)
  {
   assert(retval.size() == 3);
   assert(retval[0] == 2); // 2015/02/13
   assert(retval[1] == 3); // 2015/03/13
   assert(retval[2] == 11); // 2015/11/13
  }

 for (int month : retval) printf("%d ", month);
 
 getchar();
 
 return 0;
}