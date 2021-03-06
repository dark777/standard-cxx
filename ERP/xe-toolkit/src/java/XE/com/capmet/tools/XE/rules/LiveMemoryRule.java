/****************************************************************************
**
** Copyright (c) 2006-2007 Captive Metrics Software Corporation.
**                    All rights reserved.
**
** This file is part of the XE library for The XE Toolkit.
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

package com.capmet.tools.XE.rules;

import java.rmi.RemoteException;
import java.rmi.NotBoundException;
import java.net.MalformedURLException;
import com.capmet.metrics.cm.CmGlobal;
import com.capmet.metrics.cm.CmNames;
import com.capmet.metrics.cm.CmSysStat;
import com.capmet.metrics.cm.CmSysInfo;
import com.capmet.metrics.rmi.PerformanceHost;
import com.capmet.metrics.rmi.PerformanceField;
import com.capmet.metrics.rmi.PerformanceException;

public class LiveMemoryRule extends PureMemoryRule {

    private CmGlobal<CmSysStat>  gstat;
    private CmSysStat            stat;
    private CmGlobal<CmSysInfo>  ginfo;
    private CmSysInfo            info;
    private CmGlobal<CmNames>    gnames;
    private CmNames              names;
    private PureMemorySeedValues sv;
    private int                  pgk;
    private PerformanceField     pageFaults;

    public LiveMemoryRule() throws
        NotBoundException, MalformedURLException,
            RemoteException, PerformanceException {

        this("localhost");
    }

    public LiveMemoryRule(String remoteHost) throws
        NotBoundException, MalformedURLException,
            RemoteException, PerformanceException {

        gstat = CmGlobal.getSysStat(remoteHost);
        stat = gstat.getObject();

        ginfo = CmGlobal.getSysInfo(remoteHost);
        info = ginfo.getObject();

        gnames = CmGlobal.getNames(remoteHost);
        names = gnames.getObject();

        sv = new PureMemorySeedValues();
        sv.osFlavor = info.getOSFlavor();
        sv.clockMHz = info.cpuClockSpeed.intValue();
        sv.pageSize = info.pageSize.intValue();

        pgk = sv.pageSize / 1024;

        if (sv.osFlavor.isWindowsHost()) {
            PerformanceHost host = new PerformanceHost(remoteHost);
            String pf = host.getLocalizedName("Page Faults/sec");

            pageFaults = new PerformanceField(
                names.WindowsMemory, pf, null, remoteHost);
        } else
            pageFaults = null;

        seedValues(sv);
    }

    public void fireLiveRule() throws
        NotBoundException, MalformedURLException,
            RemoteException, PerformanceException {

        gstat.refresh();
        ginfo.refresh();

        sv.pagesPagedOut = stat.KBytesPagedOut.intValue() / pgk;

        if (pageFaults == null)
            // isUnixHost()
            sv.scanRate = stat.pagesScanned.intValue();
        else {
            // isWindowsHost()
            sv.pageFaults = stat.pageFaults.intValue();
            sv.pagesPagedIn = stat.KBytesPagedIn.intValue() / pgk;
        }

        seedValues(sv);
        firePureRule();
    }
}
