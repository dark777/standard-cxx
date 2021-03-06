/****************************************************************************
**
** Copyright (c) 2006-2007 Captive Metrics Software Corporation.
**                    All rights reserved.
**
** This file is part of the XE programs for The XE Toolkit.
**
** This file may be used under the terms of the GNU General Public
** License version 2.0 as published by the Free Software Foundation
** and appearing in the file LICENSE-GPL.txt contained within the
** same package as this file. This software is subject to a
** dual-licensing mechanism, the details of which are outlined in
** file LICENSE-DUAL.txt, also contained within this package. Be sure
** to use the correct license for your needs. To view the commercial
** license, read LICENSE-COMMERCIAL.txt also contained within this
** package.
**
** If you do not have access to these files or are unsure which license
** is appropriate for your use, please contact the sales department at
** sales@captivemetrics.com.
**
** This file is provided AS IS with NO WARRANTY OF ANY KIND, INCLUDING THE
** WARRANTY OF DESIGN, MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
**
****************************************************************************/

package com.capmet.tools.XE.programs;

import com.capmet.tools.XE.GetOpt;
import com.capmet.tools.XE.XEMessages;
import com.capmet.metrics.cm.CmSysStat;
import com.capmet.metrics.cm.CmExceptionParser;
import com.capmet.metrics.CmInteger;

/**
 * An application for monitoring useful information about a host.<p>
 *
 * Use: java SysStat [-h remote-host] [interval [iterations]]
 */
public class SysStat
{
    private final int HEADER_INTERVAL = 20;

    /**
     * The main method of the SysStat application.
     * @param args Arguments passed from the environment
     */
    public static void main(String[] args)
    {
        new SysStat(args);
    }

    // usage blurb
    private void usage()
    {
        System.err.println(XEMessages.msgs.getString("SysStat_Usage"));
    }

    private String maybeValue(CmInteger value)
    {
        if (!value.isSupported())
            return "-";
        return value.toString();
    }

    /**
     * SysStat constructor.
     * @param args Arguments passed from the environment
     */
    public SysStat(String[] args)
    {
        try
        {
            GetOpt opts = new GetOpt(args, "h:");
            String remoteHost = null;
            int iterations = 1;
            int interval = 1;
            int lineCount = 0;
            int c;

            opts.optErr = false;
            while((c = opts.getopt()) != opts.optEOF)
            {
                switch(c) {
                case 'h':
                    remoteHost = opts.getOptArg();
                    break;
                default:
                    usage();
                    return;
                }
            }

            int optind = opts.getOptIndex();
            int argCount = args.length - optind;

            switch(argCount) {
            case 0:
                break;
            case 1:
                try {
                    interval = Integer.parseInt(args[optind]);
                    iterations = 0;
                } catch(NumberFormatException e) {
                    usage();
                    return;
                }
                break;
            case 2:
                try {
                    interval = Integer.parseInt(args[optind++]);
                    iterations = Integer.parseInt(args[optind]);
                } catch(NumberFormatException e) {
                    usage();
                    return;
                }
                break;
            default:
                usage();
                return;
            }

            if (interval == 0)
                interval = 1;
            if (iterations == 0)
                iterations = Integer.MAX_VALUE;

            CmSysStat sys = new CmSysStat(remoteHost);

            do {
                if (lineCount == 0)
                {
                    System.out.println(
                        "-procs-- -mem- --pf- ----vm--- ----io--- " +
                            "-intr- -----flt----- ------cpu%-----");
                    System.out.println(
                        " r  b    %free %free  Kpi  Kpo   Kr   Kw  " +
                            "total    vcs    sys   u   k   w   i");
                    lineCount = HEADER_INTERVAL;
                }

                System.out.printf("%2d %2d    %5d %5d %4d %4d " +
                                "%4d %4d %6d %6d %6s %3d %3d %3s %3d\n",
                    sys.runnableProcesses.intValue(),
                    sys.blockedProcesses.intValue(),
                    sys.memoryFreePercent.intValue(),
                    sys.pagingFileFreePercent.intValue(),
                    sys.KBytesPagedIn.intValue(),
                    sys.KBytesPagedOut.intValue(),
                    sys.KBytesRead.intValue(),
                    sys.KBytesWritten.intValue(),
                    sys.interrupts.intValue(),
                    sys.contextSwitches.intValue(),
                    maybeValue(sys.systemCalls),
                    sys.userTimePercent.intValue(),
                    sys.systemTimePercent.intValue(),
                    maybeValue(sys.waitTimePercent),
                    sys.idleTimePercent.intValue());

                lineCount--;

                if (--iterations > 0) {
                    Thread.sleep(interval * 1000);
                    sys.refresh();
                }
            } while(iterations > 0);
        }
        catch(Exception e)
        {
            System.err.println(new CmExceptionParser(e).getMessage());
        }
    }
}
