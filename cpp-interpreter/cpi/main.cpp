#include <QtCore/QtCore>
#include <iostream>
#include "compiler.h"
#include "codegenerator.h"
#include "print.h"
#ifdef Q_OS_WIN32
# include <windows.h>
#else
# include <unistd.h>
# include <csignal>
#endif

#define CPI_VERSION_STR "2.0.0"
#define CPI_VERSION_NUMBER 0x020000

#ifdef Q_CC_MSVC
# define DEFAULT_CONFIG                                         \
    "[General]\n"                                               \
    "CXX=cl.exe\n"                                              \
    "CXXFLAGS=\n"                                               \
    "LDFLAGS=\n"                                                \
    "COMMON_INCLUDES=\n"
#else
# define DEFAULT_CONFIG                                         \
    "[General]\n"                                               \
    "### Example option for Qt5\n"                              \
    "#CXX=\n"                                                   \
    "#CXXFLAGS=-fPIC -pipe -std=c++0x -D_REENTRANT -I/usr/include/qt5\n" \
    "#LDFLAGS=-lQt5Core\n"                                      \
    "#COMMON_INCLUDES=\n"                                       \
    "\n"                                                        \
    "CXX=\n"                                                    \
    "CXXFLAGS=-fPIC -pipe -std=c++0x -D_REENTRANT\n"            \
    "LDFLAGS=\n"                                                \
    "COMMON_INCLUDES=\n"
#endif

// Entered headers and code
static QStringList headers, code;
QSettings *conf;
QStringList cppsArgs;


QString aoutName()
{
    static QString aout;
    if (aout.isEmpty()) {
        aout = QDir::tempPath() + QDir::separator();
#ifdef Q_OS_WIN32
        aout += ".cpiout" + QString::number(QCoreApplication::applicationPid()) + ".exe";
#else
        aout += ".cpi" + QString::number(QCoreApplication::applicationPid()) + ".out";
#endif
    }
    return aout;
}


#ifdef Q_OS_WIN32
static BOOL WINAPI signalHandler(DWORD ctrlType)
{
    switch (ctrlType) {
    case CTRL_C_EVENT:
    case CTRL_BREAK_EVENT:
    case CTRL_CLOSE_EVENT:
    case CTRL_LOGOFF_EVENT:
    case CTRL_SHUTDOWN_EVENT: {
        // cleanup
        if (QFileInfo(aoutName()).exists()) {
            QFile::remove(aoutName());
        }
        break; }

    default:
        return FALSE;
    }

    while (true)
        Sleep(1);

    return TRUE;
}
#else

static void signalHandler(int)
{
    // cleanup
    if (QFileInfo(aoutName()).exists()) {
        QFile::remove(aoutName());
    }
    _exit(0);
}

static void watchUnixSignal(int sig)
{
    if (sig < NSIG) {
        struct sigaction sa;
        memset(&sa, 0, sizeof(sa));
        sa.sa_flags = SA_RESTART;
        sa.sa_handler = signalHandler;
        sigaction(sig, &sa, 0);
    }
}
#endif


static QString isSetFileOption()
{
    QString ret;
    for (int i = 1; i < QCoreApplication::arguments().length(); i++) {
        auto opt = QCoreApplication::arguments()[i];
        if (!opt.startsWith("-") && QFileInfo(opt).exists()) {
            ret = opt;
            cppsArgs = QCoreApplication::arguments().mid(i+1);
            break;
        }
    }
    return ret;
}


static void showHelp()
{
    char help[] =
        " .conf        Display the current values for various settings.\n" \
        " .help        Display this help.\n"                               \
        " .rm LINENO   Remove the code of the specified line number.\n"    \
        " .clear       Clear the code all.\n"                              \
        " .show        Show the current source code.\n"                    \
        " .quit        Exit this program.\n";
    print() << help;
}


static void showConfigs(const QSettings &conf)
{
    QStringList confkeys;
    confkeys << "CXX" << "CXXFLAGS" << "LDFLAGS" << "COMMON_INCLUDES";

    QStringList configs = conf.allKeys();
    for (QStringListIterator it(conf.allKeys()); it.hasNext(); ) {
        const QString &key = it.next();
        if (confkeys.contains(key))
            printf("%s=%s\n", qPrintable(key), qPrintable(conf.value(key).toString()));
    }
}


static void deleteLine(int n)
{
    int h = headers.count();
    int c = code.count();

    if (n > 0) {
        if (n <= h) {
            headers.removeAt(n - 1);
        } else if (n <= h + c) {
            code.removeAt(n - h - 1);
        } else {
            // ignore
        }
    }
}


static void deleteLines(const QList<int> &numbers)
{
    QList<int> list = numbers.toSet().toList(); // removes duplicates
    qSort(list.begin(), list.end(), qGreater<int>());  // sort
    for (QListIterator<int> it(list); it.hasNext(); ) {
        deleteLine(it.next());
    }
}


static void showCode()
{
    int num = 1;
    if (!headers.isEmpty()) {
        for (int i = 0; i < headers.count(); ++i)
            printf("%3d| %s\n", num++, qPrintable(headers.at(i)));
        printf("    --------------------\n");
    }
    for (int i = 0; i < code.count(); ++i)
        printf("%3d| %s\n", num++, qPrintable(code.at(i)));
}


static int interpreter()
{
    print() << "Cpi " << CPI_VERSION_STR << endl;
    print() << "Type \".help\" for more information." << endl;
    print() << "Loaded INI file: " << conf->fileName() << endl;
    print().flush();
    Compiler compiler;

    QStringList includes = conf->value("COMMON_INCLUDES").toString().split(" ", QString::SkipEmptyParts);
    for (QStringListIterator i(includes); i.hasNext(); ) {
        QString s = i.next().trimmed();
        if (!s.isEmpty()) {
            if (s.startsWith("<") || s.startsWith('"')) {
                headers << QString("#include ") + s;
            } else {
                headers << QString("#include <") + s + ">";
            }
        }
    }

    bool end = false;
    auto readfunc = [&]() {
        // read and write to the process
        QString str;
        std::string s;

        if (std::getline(std::cin, s)) {
            str = QString::fromStdString(s);
        } else {
            end = true;
            return;
        }

        auto line = QString(str).trimmed();
        if (line == ".quit" || line == ".q") {
            end = true;
            return;
        }

        class PromptOut {
        public:
            ~PromptOut() { print() << "cpi> " << flush; }
        } promptOut;

        if (line == ".help" || line == "?") {  // shows help
            showHelp();
            return;
        }

        if (line == ".show" || line == ".code") {  // shows code
            showCode();
            return;
        }

        if (line == ".conf") {  // shows configs
            showConfigs(*conf);
            return;
        }

        if (line.startsWith(".del ") || line.startsWith(".rm ")) { // Deletes code
            int n = line.indexOf(' ');
            line.remove(0, n + 1);
            QStringList list = line.split(QRegExp("[,\\s]"), QString::SkipEmptyParts);

            QList<int> numbers; // line-numbers
            for (QStringListIterator it(list); it.hasNext(); ) {
                const QString &s = it.next();
                bool ok;
                int n = s.toInt(&ok);
                if (ok && n > 0)
                    numbers << n;
            }
            deleteLines(numbers);
            showCode();
            return;
        }

        if (line == ".clear") {
            headers.clear();
            code.clear();
            return;
        }

        if (line.startsWith('#') || line.startsWith("using ")) {
            headers << line;
        } else {
            if (!line.isEmpty()) {
                code << line;
            }
        }

        if (code.isEmpty())
            return;

        // compile
        CodeGenerator cdgen(headers.join("\n"), code.join("\n"));
        QString src = cdgen.generateMainFunc();

        int cpl = compiler.compileAndExecute(src);
        if (cpl) {
            // compile only once more
            src = cdgen.generateMainFuncSafe();
            cpl = compiler.compileAndExecute(src);
        }

        if (cpl) {
            compiler.printContextCompilationError();
        }
    };
    print() << "cpi> " << flush;

#ifdef Q_OS_WIN32
    HANDLE h = GetStdHandle(STD_INPUT_HANDLE);
    while (!end) {
        if (WaitForSingleObject(h, 50) == WAIT_OBJECT_0) {
            readfunc();
        }
    }
#else
    QSocketNotifier notifier(fileno(stdin), QSocketNotifier::Read);
    QObject::connect(&notifier, &QSocketNotifier::activated, readfunc);
    while (!end) {
        QThread::msleep(50);
        qApp->processEvents();
    }
#endif
    return 0;
}


int main(int argv, char *argc[])
{
    QCoreApplication app(argv, argc);

#if (defined Q_OS_WIN32) || (defined Q_OS_DARWIN)
    conf = new QSettings(QSettings::IniFormat, QSettings::UserScope, "cpi/cpi");
#else
    conf = new QSettings(QSettings::NativeFormat, QSettings::UserScope, "cpi/cpi");
#endif

    QFile confFile(conf->fileName());
    if (!confFile.exists()) {
        if (confFile.open(QIODevice::WriteOnly | QIODevice::Truncate)) {
            confFile.write(DEFAULT_CONFIG);
            confFile.close();
        }
        conf->sync();
    }

#ifdef Q_OS_WIN32
    SetConsoleCtrlHandler(signalHandler, TRUE);
#else
    watchUnixSignal(SIGTERM);
    watchUnixSignal(SIGINT);
#endif

    QString file = isSetFileOption();

    if (!file.isEmpty()) {
        Compiler compiler;
        int ret = compiler.compileFileAndExecute(file);
        if (ret) {
            compiler.printLastCompilationError();
        }
        return ret;
    }

    int ret = interpreter();
    delete conf;
    return ret;
}
