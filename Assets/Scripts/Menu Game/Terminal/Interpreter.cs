using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class Interpreter : MonoBehaviour
{
    List<string> response = new List<string>();
    public QuestManager questManager;
    private QuestSO activeQuest;
    private bool isModeMonitor = false;

    private string handshake;


    // wifi 1: Area1
    private string bssid1 = "16:82:A2:CA:D2:D1";
    private int channel1 = 11;
    private string password1 = "password1";

    // wifi 2: Area2
    private string bssid2 = "51:11:25:AB:A6:42";
    private int channel2 = 97;
    private string password2 = "password2";

    private int currentChannel;
    private string currentBssid;

    private bool aircrackInstalled = false;
    private bool snortInstalled = false;
    private bool nmapInstalled = false;
    private bool netcatInstalled = false;
    private bool packetSnort1 = false;
    private bool packetSnort2 = false;
    private bool handshakeFile = false;

    private int fileDownload = 0;


    private List<string> handshakeList = new List<string>();

    private string[] args;
    private bool ipAdd;
    private int indexQuest;

    // Deklarasi delegate untuk loading selesai
    public delegate void LoadingCompletedDelegate();
    // Deklarasi event untuk loading selesai
    //public static event LoadingCompletedDelegate OnLoadingCompleted;

    private void Awake()
    {
        //questManager = QuestManager.Instance;
        if (questManager != null)
        {
            Debug.Log("questManager manager ditemukan");
            //questManager.AktifQuest(indexQuest);

        }
    }
    private void Start()
    {
        LoadInstalledTools();
    }

    private void MengaktifkanQuest()
    {
        activeQuest = questManager.questYangAktif;
    }

    private void SaveInstalledTools()
    {
        PlayerPrefs.SetInt("aircrack", aircrackInstalled ? 1 : 0);
        PlayerPrefs.SetInt("snort", snortInstalled ? 1 : 0);
        PlayerPrefs.SetInt("nmap", nmapInstalled ? 1 : 0);
        PlayerPrefs.SetInt("netcat", netcatInstalled ? 1 : 0);
        PlayerPrefs.SetInt("packetsnort1", packetSnort1 ? 1 : 0);
        PlayerPrefs.SetInt("packetsnort2", packetSnort2 ? 1 : 0);
        PlayerPrefs.SetInt("handshakefile", handshakeFile ? 1 : 0);
        PlayerPrefs.SetInt("modemonitor", isModeMonitor ? 1 : 0);

        PlayerPrefs.SetInt("terminalnetcat", TerminalManager.netcat ? 1 : 0);
        PlayerPrefs.SetInt("terminalmeterpreter", TerminalManager.meterpreter ? 1 : 0);

        PlayerPrefs.SetInt("filedownload", fileDownload);
        PlayerPrefs.SetString("filehandshake", handshake);

        PlayerPrefs.SetInt("currentchannel", currentChannel);
        PlayerPrefs.SetString("currentbssid", currentBssid);

        PlayerPrefs.Save();
    }

    private void LoadInstalledTools()
    {
        // Memuat nilai boolean
        aircrackInstalled = PlayerPrefs.GetInt("aircrack", 0) == 1;
        snortInstalled = PlayerPrefs.GetInt("snort", 0) == 1;
        nmapInstalled = PlayerPrefs.GetInt("nmap", 0) == 1;
        netcatInstalled = PlayerPrefs.GetInt("netcat", 0) == 1;
        packetSnort1 = PlayerPrefs.GetInt("packetsnort1", 0) == 1;
        packetSnort2 = PlayerPrefs.GetInt("packetsnort2", 0) == 1;
        handshakeFile = PlayerPrefs.GetInt("handshakefile", 0) == 1;
        isModeMonitor = PlayerPrefs.GetInt("modemonitor", 0) == 1;

        TerminalManager.netcat = PlayerPrefs.GetInt("terminalnetcat", 0) == 1;
        TerminalManager.meterpreter = PlayerPrefs.GetInt("terminalmeterpreter", 0) == 1;

        // Memuat nilai int dan string
        fileDownload = PlayerPrefs.GetInt("filedownload", 0);
        handshake = PlayerPrefs.GetString("filehandshake", "");

        currentChannel = PlayerPrefs.GetInt("currentchannel", 0);
        currentBssid = PlayerPrefs.GetString("currentbssid", "");
    }


    public List<string> Interpret(string userInput)
    {
        //QuestSO[] quests = Resources.LoadAll<QuestSO>("Quest");
        MengaktifkanQuest();

        response.Clear();

        args = userInput.Split();
        ipAdd = IsIPAddressValid(args.Length > 1 ? args[1] : "");
        //string handshake;


        // Function IsIPAdressValid
        // Untuk mengecek apakah ip yang dimasukkan sudah sesuai dengan format
       
        if (activeQuest == null)
        {
            fileDownload = 0;
            SaveInstalledTools();
            QuestStart();
        }

        else if (activeQuest != null)
        {
            if (activeQuest.questID == "0_Start")
            {
                QuestStart();

            }
            else if (activeQuest.questID == "1_UserPref")
            {
                //nothing
            }
            else if (activeQuest.questID == "2_OpDmail")
            {
                //nothing
            }
            else if (activeQuest.questID == "3_InSnortSniffer")
            {
                QuestSnort();
            }
            else if (activeQuest.questID == "4_InSnortPckt")
            {
                QuestSnort();
            }
            else if (activeQuest.questID == "5_SnortSniffer")
            {
                QuestSnort();
            }
            else if (activeQuest.questID == "6_SnortPacket")
            {
                SnortPacket();
            }
            else if (activeQuest.questID == "7_InNmap")
            {
                QuestNMAP();
            }
            else if (activeQuest.questID == "8_Nmap1")
            {
                QuestNMAPScanIP();
            }
            else if (activeQuest.questID == "9_Nmap2")
            {
                QuestNMAPScanPort();
            }
            else if (activeQuest.questID == "10_InExploit")
            {
                QuestIntroExploit();
            }
            else if (activeQuest.questID == "11_Exploit1")
            {
                QuestExploit1();
            }
            else if (activeQuest.questID == "12_Exploit2")
            {
                QuestExploit2();
            }
            else if (activeQuest.questID == "13_InOS")
            {
                QuestOSIntro();
            }
            else if (activeQuest.questID == "14_OS")
            {
                QuestOS();
            }
            else if (activeQuest.questID == "15_NetScan")
            {
                QuestNS();
            }
            else if (activeQuest.questID == "16_InNc")
            {
                QuestIntroNc();
            }
            else if (activeQuest.questID == "17_Nc1")
            {
                QuestNc1();
            }
            else if (activeQuest.questID == "18_Nc2")
            {
                QuestNc2();
            }
            else if (activeQuest.questID == "19_Nc3")
            {
                QuestNc3();
            }
        }
        else
        {
            response.Add("ERROR: Command " + args[0] + " tidak ditemukan");
        }

        return response;
    }

    private bool IsIPAddressValid(string input)
    {
        string ipAddressPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, ipAddressPattern);
    }

    //done
    private void QuestStart()
    {
        // install tools
        if (args[0] == "apt-get")
        {
            if (args.Length == 1)
            {
                response.Add("ERROR: Command " + args[0] + " tidak memiliki parameter");
            }
            else if (args[1] == "install")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Tools belum dimasukkan");
                }
                else if (args[2] == "aircrack-ng")
                {

                    response.Add("Reading package lists… Done");
                    response.Add("Building dependency tree");
                    response.Add("Reading state information… Done");
                    response.Add("aircrack-ng is already the newest version.");
                    response.Add("0 upgraded, 0 newly installed, 0 to remove and 0 not upgraded.");
                    response.Add("");
                    aircrackInstalled = true;
                    SaveInstalledTools();
                    Debug.Log("aircrack " + aircrackInstalled);
                }
                else
                {
                    response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }

        // wifi crack
        else if (args[0] == "iwconfig")
        {
            if (isModeMonitor)
            {
                // Jika status mode adalah monitor, kirim pesan mode monitor
                response.Add("Wlan0mon		IEEE 802.11 Mode:Monitor Frequency:2.457 Ghz Tx-Power=20 dBm");
                //StartCoroutine(Loading(1f));
                response.Add("\t\tRetry short limit:7 RTS thr:off Fragment thr:off");
                response.Add("\t\tPower Management:off");
                response.Add("");
            }
            else
            {
                // Jika status mode adalah managed, kirim pesan mode managed
                response.Add("Wlan0		IEEE 802.11 ESSID:off/any");
                response.Add("\t\tMode: Managed Access Point: Not - Associated Tx - Power = 20 dBm");
                response.Add("\t\tRetry short limit: 7 RTS thr: off Fragment thr: off");
                response.Add("\t\tEncryption key: off");
                response.Add("\t\tPower Management: off");
                response.Add("");
            }
        }
        else if (args[0] == "airmon-ng" && aircrackInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format: airmon-ng [start/stop] [interface]");
                response.Add("");
            }
            else if (args.Length > 1)
            {
                if (args[1] == "start")
                {
                    if (args.Length == 2)
                    {
                        response.Add("ERROR: Interface belum dimasukkan");
                    }
                    else if (args[2] == "wlan0")
                    {
                        // Set status mode menjadi monitor
                        isModeMonitor = true;
                        SaveInstalledTools();
                        Debug.Log("mode monitor " + isModeMonitor);
                        response.Add("PHY\tInterface\tDriver\tChipset");
                        response.Add("Phy0\twlan0\tath9k_htc\tQualcom Atheros Communications AR9271 802.11n");
                        response.Add("\t\t\t(mac80211 monitor mode vif enabled for [phy0]wlan0 on [phy0]wlan0mon)");
                        response.Add("\t\t\t(mac80211 station mode vif disabled for [phy0]wlan0)");
                        response.Add("");
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[2] + " tidak ditemukan"); ;
                    }
                }
                else if (args[1] == "stop")
                {
                    if (args.Length == 2)
                    {
                        response.Add("ERROR: Interface belum dimasukkan");
                    }
                    else if (args[2] == "wlan0mon")
                    {
                        // Set status mode menjadi monitor
                        isModeMonitor = false;
                        SaveInstalledTools();
                        Debug.Log("mode monitor " + isModeMonitor);
                        response.Add("PHY\tInterface\tDriver\tChipset");
                        response.Add("Phy0\twlan0\tath9k_htc\tQualcom Atheros Communications AR9271 802.11n");
                        response.Add("\t\t\t(mac80211 station mode vif enabled for [phy0]wlan0)");
                        response.Add("");
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[2] + " tidak ditemukan"); ;
                    }
                }
                else
                {
                    response.Add("ERROR: Command " + args[1] + " tidak ditemukan"); ;
                }
            }

        }
        else if (args[0] == "airodump-ng" && aircrackInstalled && isModeMonitor)
        {
            if (args.Length == 1)
            {
                response.Add("Format:");
                response.Add("Untuk memantau jaringan wifi:");
                response.Add("airodump-ng [interface]");
                response.Add("Untuk menangkap file handshake:");
                response.Add("airodump-ng -w [nama file handshake] -c [channel] -bssid [bssid]");
                response.Add("");
            }
            else if (args.Length == 2)
            {
                if (args[1] == "wlan0mon")
                {
                    response.Add(" CH 8 ][ Elapsed: 18 s ][ 2024-03-22 14:02");
                    response.Add("BSSID\t\tPWR\tBeacons\tCH\tMB\tENC\tCIPHER\tAUTH\tESSID");
                    response.Add(bssid1 + "\t-56\t16\t" + channel1 + "\t130\tWPA2\tCCMP\tPSK\tWifi ke satu");
                    response.Add(bssid2 + "\t-72\t10\t" + channel2 + "\t127\tWPA2\tCCMP\tPSK\tWifi ke dua");
                    response.Add("");
                    response.Add("BSSID\t\tSTATION\t\tPWR\tRate\tLost\tFrames\tNotes\tProbes");
                    response.Add("A2:15:91:B3:2D:FF\t12:B4:1A:8D:C6:54\t-86\t0-1\t0\t2");
                    response.Add("33:12:BA:15:81:AB\tBA:91:65:C1:BB:71\t-72\t0-1e\t0\t1");
                    response.Add("Quitting...");
                    response.Add("");
                }
                else
                {
                    response.Add("ERROR: Interface " + args[1] + " tidak ditemukan");
                }
            }
            else if (args.Length > 2)
            {
                if (args[1] == "-w")
                {
                    handshake = args[2];
                    SaveInstalledTools();
                    if (args[3] == "-c")
                    {
                        if (int.TryParse(args[4], out currentChannel))
                        {
                            SaveInstalledTools();
                            if (currentChannel == channel1 || currentChannel == channel2)
                            {
                                if (args[5] == "-bssid")
                                {
                                    currentBssid = args[6];
                                    SaveInstalledTools();
                                    if (currentBssid == bssid1 || currentBssid == bssid2)
                                    {
                                        if (currentChannel == channel1 && currentBssid == bssid1)
                                        {
                                            handshakeFile = true;
                                            SaveInstalledTools();
                                            response.Add("BSSID\t\tPWR\tBeacons\tCH\tMB\tENC\tCIPHER\tAUTH\tESSID");
                                            response.Add(bssid1 + "\t-56\t16\t" + channel1 + "\t130\tWPA2\tCCMP\tPSK\tWifi ke satu");
                                            response.Add("");
                                            response.Add("BSSID\t\tSTATION\t\tPWR\tRate\tLost\tFrames\tNotes\tProbes");
                                            response.Add(bssid1 + "\tBC:51:81:AC:45:91\t-56\t0-1e\t0\t3");
                                            response.Add(bssid1 + "\t45:19:20:A0:1C:54\t-92\t0-6e\t0\t1");
                                            response.Add("Quitting...");
                                            response.Add("");
                                        }
                                        else if (currentChannel == channel2 && currentBssid == bssid2)
                                        {
                                            handshakeFile = true;
                                            SaveInstalledTools();
                                            response.Add("BSSID\t\tPWR\tBeacons\tCH\tMB\tENC\tCIPHER\tAUTH\tESSID");
                                            response.Add(bssid2 + "\t-72\t10\t" + channel2 + "\t127\tWPA2\tCCMP\tPSK\tWifi ke dua");
                                            response.Add("");
                                            response.Add("BSSID\t\tSTATION\t\tPWR\tRate\tLost\tFrames\tNotes\tProbes");
                                            response.Add(bssid2 + "\tBC:51:81:AC:45:91\t-56\t0-1e\t0\t3");
                                            response.Add(bssid2 + "\t45:19:20:A0:1C:54\t-92\t0-6e\t0\t1");
                                            response.Add("Quitting...");
                                            response.Add("");
                                        }
                                        else
                                        {
                                            response.Add("ERROR: Channel dan BSSID tidak cocok");
                                        }
                                    }
                                    else
                                    {
                                        response.Add("ERROR: BSSID tidak ditemukan");
                                    }
                                }
                                else
                                {
                                    response.Add("ERROR: Command " + args[5] + " tidak ditemukan");
                                }
                            }
                            else
                            {
                                response.Add("ERROR: Channel tidak ditemukan");
                            }
                        }
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[3] + " tidak ditemukan");
                    }

                }
                else
                {
                    response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
                }
            }
        }
        else if (args[0] == "aireplay-ng" && aircrackInstalled && isModeMonitor)
        {
            if (args.Length == 1)
            {
                response.Add("Format: aireplay-ng --deauth -0 -a [bssid] [interface]");
                response.Add("");
            }
            else if (args.Length > 1)
            {
                if (args[1] == "--deauth")
                {
                    if (args[2] == "0")
                    {
                        if (args[3] == "-a")
                        {
                            if (args[4] == currentBssid)
                            {
                                SaveInstalledTools();
                                if (args.Length < 6 || args[5] == null)
                                {
                                    response.Add("ERROR: Interface tidak ditemukan");
                                }
                                else if (args[5] == "wlan0mon")
                                {
                                    response.Add("14:18:43 Waiting for beacon frame (BSSID: " + currentBssid + ") on channel " + currentChannel);
                                    response.Add("NB: this attack is more effective when targeting");
                                    response.Add("a connected wireless client (-c <client’s mac>).");
                                    response.Add("14:18:43 Sending DeAuth (code 7) to broadcast – BSSID: [" + currentBssid + "]");
                                    response.Add("14:18:43 Sending DeAuth (code 7) to broadcast – BSSID: [" + currentBssid + "]");
                                    response.Add("14:18:43 Sending DeAuth (code 7) to broadcast – BSSID: [" + currentBssid + "]");
                                    response.Add("14:18:43 Sending DeAuth (code 7) to broadcast – BSSID: [" + currentBssid + "]");
                                    response.Add("14:18:43 Sending DeAuth (code 7) to broadcast – BSSID: [" + currentBssid + "]");
                                    response.Add("");
                                }
                                else
                                {
                                    response.Add("ERROR: Interface " + args[5] + " tidak ditemukan");
                                }
                            }
                            else
                            {
                                response.Add("ERROR : BSSID yang dimasukkan salah");
                            }
                        }
                        else
                        {
                            response.Add("ERROR: Command " + args[3] + " tidak ditemukan");
                        }
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                    }
                }
                else
                {
                    response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
                }
            }
        }
        else if (args[0] == "aircrack-ng" && aircrackInstalled && isModeMonitor)
        {
            if (args.Length == 1)
            {
                response.Add("Format: aircrack-ng [nama file handshake] -w [file wordlists]");
                response.Add("");
            }
            else if (args.Length > 1)
            {
                if (args[1] == handshake + "-01.cap")
                {
                    if (args[2] == "-w")
                    {
                        if (args[3] == "wordlists.txt")
                        {
                            response.Add("				   Aircrack-ng 1.6");
                            response.Add("[00:00:00] 12/17 key tested (613.87 k/s");
                            response.Add("");
                            response.Add("Time left: 0 seconds\t\t\t\t\t\t\t\t        70.59%");
                            if (currentBssid == bssid1)
                            {
                                response.Add("\t\t\t\tKEY FOUND! [  " + password1 + "  ]");
                            }
                            else if (currentBssid == bssid2)
                            {
                                response.Add("\t\t\t\tKEY FOUND! [  " + password2 + "  ]");
                            }
                            response.Add("");
                            response.Add("Master Key\t\t: B3 A8 51 72 AC 91 53 C3 4B FA 71 D9 77 58 A1 21");
                            response.Add("\t\t  1B 78 51 AC F1 83 56 C1 D9 73 A6 82 31 B8 91 52");
                            response.Add("");
                            response.Add("Transient Key\t: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                            response.Add("\t\t  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                            response.Add("\t\t  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                            response.Add("");
                            response.Add("EAPOL HMAC\t\t: B3 71 52 41 9A B3 61 C8 74 23 A2 7C 19 65 C3 3A");
                            response.Add("");

                            handshakeFile = false;
                            SaveInstalledTools();
                        }
                        else
                        {
                            response.Add("ERROR: Command " + args[3] + " tidak ditemukan");
                        }
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                    }
                }
                else
                {
                    response.Add("ERROR: File " + args[1] + " tidak ditemukan");
                }
            }
        }
        else
        {
            //response.Add("ERROR: Command " + args[0] + " tidak ditemukan");
            InputUmum();
        }

    }


    //done
    private void QuestSnort()
    {
        // install tools
        if (args[0] == "apt-get")
        {
            if (args.Length == 1)
            {
                response.Add("ERROR: Command " + args[0] + " tidak memiliki parameter");
            }
            else if (args[1] == "install")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Tools belum dimasukkan");
                }
                else if (args[2] == "snort")
                {
                    response.Add("Reading package lists… Done");
                    response.Add("Building dependency tree");
                    response.Add("Reading state information… Done");
                    response.Add("snort is already the newest version.");
                    response.Add("0 upgraded, 0 newly installed, 0 to remove and 0 not upgraded.");
                    response.Add("");
                    snortInstalled = true;
                    SaveInstalledTools();
                    Debug.Log("aircrack " + snortInstalled);
                }
                else
                {
                    response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }

        else if (args[0] == "snort" && snortInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format: snort [parameter]");
                response.Add("");
            }
            else if (args.Length == 2)
            {
                if (args[1] == "-v")
                {
                    response.Add("Running in packet dump mode");
                    response.Add("");
                    response.Add("        --== Initializing Snort ==--");
                    response.Add("Initializing Output Plugins!");
                    response.Add("***");
                    response.Add("*** interface device lookup found: eth0");
                    response.Add("***");
                    response.Add("Initializing Network Interface eth0");
                    response.Add("Decoding Ethernet on interface eth0");
                    response.Add("");
                    response.Add("        --== Initialization Complete ==--");
                    response.Add("");
                    response.Add("   ,,_     -*> Snort! <*-");
                    response.Add("  o\")~Version 2.8.5.2(Build 121) ");
                    response.Add("   ''''    By Martin Roesch & The Snort Team: http://www.snort.org/contact#team");
                    response.Add("           Copyright (C) 1998-2009 Sourcefire, Inc., et al.");
                    response.Add("           Using PCRE version: 8.02 2010-03-19");
                    response.Add("");
                    response.Add("Not Using PCAP_FRAMES");
                    response.Add("05/11-18:23:23.985617 192.168.10.20 -> 192.168.10.10");
                    response.Add("ICMP TTL:64 TOS:0X0 ID:48904 IpLen:20 DgmLen:84 DF");
                    response.Add("Type:8 Code:0 ID:1709 Seq:1 ECHO");
                    response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
                    response.Add("");
                    response.Add("05/11-18:23:23.985634 192.168.10.10 -> 192.168.10.20");
                    response.Add("ICMP TTL:64 TOS:0X0 ID:63182 IpLen:20 DgmLen:84 ");
                    response.Add("Type:0 Code:0 ID:1709 Seq:1 ECHO REPLY");
                    response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
                    response.Add("");
                    response.Add("05/11-18:23:24.987910 192.168.10.20 -> 192.168.10.10");
                    response.Add("ICMP TTL:64 TOS:0X0 ID:49136 IpLen:20 DgmLen:84 DF");
                    response.Add("Type:8 Code:0 ID:1709 Seq:2 ECHO");
                    response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
                    response.Add("");
                    response.Add("05/11-18:23:24.987927 192.168.10.10 -> 192.168.10.20");
                    response.Add("ICMP TTL:64 TOS:0X0 ID:63183 IpLen:20 DgmLen:84 ");
                    response.Add("Type:0 Code:0 ID:1709 Seq:2 ECHO REPLY");
                    response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
                    response.Add("Action Stats:");
                    response.Add("ALERTS: 0");
                    response.Add("LOGGED: 0");
                    response.Add("PASSED: 0");
                    response.Add("===========================================================");
                    response.Add("Snort Exiting");
                    response.Add("");

                }
                else if (args[1] == "-vd")
                {
                    response.Add("Running in packet dump mode");
                    response.Add("");
                    response.Add("        --== Initializing Snort ==--");
                    response.Add("Initializing Output Plugins!");
                    response.Add("***");
                    response.Add("*** interface device lookup found: eth0");
                    response.Add("***");
                    response.Add("Initializing Network Interface eth0");
                    response.Add("Decoding Ethernet on interface eth0");
                    response.Add("");
                    response.Add("        --== Initialization Complete ==--");
                    response.Add("");
                    response.Add("   ,,_     -*> Snort! <*-");
                    response.Add("  o\")~Version 2.8.5.2(Build 121) ");
                    response.Add("   ''''    By Martin Roesch & The Snort Team: http://www.snort.org/contact#team");
                    response.Add("           Copyright (C) 1998-2009 Sourcefire, Inc., et al.");
                    response.Add("           Using PCRE version: 8.02 2010-03-19");
                    response.Add("");
                    response.Add("Not Using PCAP_FRAMES");
                    response.Add("05/11-18:23:58.748116 192.168.10.20 -> 192.168.10.10");
                    response.Add("ICMP TTL:64 TOS:0X0 ID:53791 IpLen:20 DgmLen:84 DF");
                    response.Add("Type:8 Code:0 ID:1709 Seq:35 ECHO");
                    response.Add("02 F0 B8 9E B6 7F 02 C8 85 B5 5A AA 08 00 45 00  ..........Z...E.");
                    response.Add("00 54 89 DA 40 00 40 06 99 0A 0A 64 01 CA 0A 0A  .T..@.@....d....");
                    response.Add("01 88 84 D4 00 50 B7 CC C0 31 21 CE B6 7C 80 18  .....P...1!..|..");
                    response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
                    response.Add("");
                    response.Add("05/11-18:23:58.748131 192.168.10.10 -> 192.168.10.20");
                    response.Add("ICMP TTL:64 TOS:0X0 ID:63216 IpLen:20 DgmLen:84 ");
                    response.Add("Type:0 Code:0 ID:1709 Seq:35 ECHO REPLY");
                    response.Add("02 F0 B8 9E B6 7F 02 C8 85 B5 5A AA 08 00 45 00  ..........Z...E.");
                    response.Add("00 54 89 DA 40 00 40 06 99 0A 0A 64 01 CA 0A 0A  .T..@.@....d....");
                    response.Add("01 88 84 D4 00 50 B7 CC C0 31 21 CE B6 7C 80 18  .....P...1!..|..");
                    response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
                    response.Add("");
                    response.Add("05/11-18:23:59.771735 192.168.10.20 -> 192.168.10.10");
                    response.Add("ICMP TTL:64 TOS:0X0 ID:53932 IpLen:20 DgmLen:84 DF");
                    response.Add("Type:8 Code:0 ID:1709 Seq:36 ECHO");
                    response.Add("02 F0 B8 9E B6 7F 02 C8 85 B5 5A AA 08 00 45 00  ..........Z...E.");
                    response.Add("00 54 89 DA 40 00 40 06 99 0A 0A 64 01 CA 0A 0A  .T..@.@....d....");
                    response.Add("01 88 84 D4 00 50 B7 CC C0 31 21 CE B6 7C 80 18  .....P...1!..|..");
                    response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
                    response.Add("");
                    response.Add("05/11-18:23:59.771753 192.168.10.10 -> 192.168.10.20");
                    response.Add("ICMP TTL:64 TOS:0X0 ID:63217 IpLen:20 DgmLen:84 ");
                    response.Add("Type:0 Code:0 ID:1709 Seq:36 ECHO REPLY");
                    response.Add("02 F0 B8 9E B6 7F 02 C8 85 B5 5A AA 08 00 45 00  ..........Z...E.");
                    response.Add("00 54 89 DA 40 00 40 06 99 0A 0A 64 01 CA 0A 0A  .T..@.@....d....");
                    response.Add("01 88 84 D4 00 50 B7 CC C0 31 21 CE B6 7C 80 18  .....P...1!..|..");
                    response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
                    response.Add("");
                    response.Add("Snort Exiting");
                    response.Add("");
                }
                else
                {
                    response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
                }
            }
            else if (args.Length > 2)
            {
                if (args[1] == "-dev")
                {
                    if (args.Length == 2)
                    {
                        response.Add("ERROR: Command tidak ditemukan");
                    }
                    else if (args[2] == "-i")
                    {
                        if (args.Length == 3)
                        {
                            response.Add("ERROR: Command tidak ditemukan");
                        }
                        else if (args[3] == "eth0")
                        {
                            if (args.Length == 4)
                            {
                                response.Add("ERROR: Command tidak ditemukan");
                            }
                            else if (args[4] == "-l")
                            {
                                if (args.Length == 5)
                                {
                                    response.Add("ERROR: Command tidak ditemukan");
                                }
                                else if (args[5] == "/var/log/snort/")
                                {
                                    packetSnort1 = true;
                                    SaveInstalledTools();
                                    response.Add("Running in packet logging mode");
                                    response.Add("");
                                    response.Add("        --== Initializing Snort ==--");
                                    response.Add("Initializing Output Plugins!");
                                    response.Add("***");
                                    response.Add("*** interface device lookup found: eth0");
                                    response.Add("***");
                                    response.Add("Initializing Network Interface eth0");
                                    response.Add("Decoding Ethernet on interface eth0");
                                    response.Add("");
                                    response.Add("        --== Initialization Complete ==--");
                                    response.Add("");
                                    response.Add("   ,,_     -*> Snort! <*-");
                                    response.Add("  o\")~Version 2.8.5.2(Build 121) ");
                                    response.Add("   ''''    By Martin Roesch & The Snort Team: http://www.snort.org/contact#team");
                                    response.Add("           Copyright (C) 1998-2009 Sourcefire, Inc., et al.");
                                    response.Add("           Using PCRE version: 8.02 2010-03-19");
                                    response.Add("");
                                    response.Add("Not Using PCAP_FRAMES");
                                    response.Add("===========================================================");
                                    response.Add("Snort Exiting");
                                    response.Add("");
                                }
                                else
                                {
                                    response.Add("ERROR: Command " + args[5] + " tidak ditemukan");
                                }
                            }
                            else
                            {
                                response.Add("ERROR: Command " + args[4] + " tidak ditemukan");
                            }
                        }
                        else
                        {
                            response.Add("ERROR: Command " + args[3] + " tidak ditemukan");
                        }

                    }
                    else if (args[2] == "-r")
                    {
                        if (args.Length == 3)
                        {
                            response.Add("ERROR: Command tidak ditemukan");
                        }
                        else if (args[3] == "/var/log/snort/snort.log.123")
                        {
                            packetSnort1 = false;
                            SaveInstalledTools();
                            OutputSnort();
                        }
                        else
                        {
                            response.Add("ERROR: Command " + args[3] + " tidak ditemukan");
                        }
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                    }
                }
                else
                {
                    response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
                }
            }
        }
        else
        {
            InputUmum();
        }
    }

    //done
    private void SnortPacket()
    {
        if (args[0] == "snort")
        {
            if (args.Length == 1)
            {
                response.Add("Format: snort [parameter]");
                response.Add("");
            }
            else if (args.Length > 2)
            {
                if (args[1] == "-dev")
                {
                    if (args.Length == 2)
                    {
                        response.Add("ERROR: Command tidak ditemukan");
                    }
                    else if (args[2] == "-i")
                    {
                        if (args.Length == 3)
                        {
                            response.Add("ERROR: Command tidak ditemukan");
                        }
                        else if (args[3] == "eth0")
                        {
                            if (args.Length == 4)
                            {
                                response.Add("ERROR: Command tidak ditemukan");
                            }
                            else if (args[4] == "-l")
                            {
                                if (args.Length == 5)
                                {
                                    response.Add("ERROR: Command tidak ditemukan");
                                }
                                else if (args[5] == "/var/log/snort/")
                                {
                                    packetSnort2 = true;
                                    SaveInstalledTools();
                                    response.Add("Running in packet logging mode");
                                    response.Add("");
                                    response.Add("        --== Initializing Snort ==--");
                                    response.Add("Initializing Output Plugins!");
                                    response.Add("***");
                                    response.Add("*** interface device lookup found: eth0");
                                    response.Add("***");
                                    response.Add("Initializing Network Interface eth0");
                                    response.Add("Decoding Ethernet on interface eth0");
                                    response.Add("");
                                    response.Add("        --== Initialization Complete ==--");
                                    response.Add("");
                                    response.Add("   ,,_     -*> Snort! <*-");
                                    response.Add("  o\")~Version 2.8.5.2(Build 121) ");
                                    response.Add("   ''''    By Martin Roesch & The Snort Team: http://www.snort.org/contact#team");
                                    response.Add("           Copyright (C) 1998-2009 Sourcefire, Inc., et al.");
                                    response.Add("           Using PCRE version: 8.02 2010-03-19");
                                    response.Add("");
                                    response.Add("Not Using PCAP_FRAMES");
                                    response.Add("===========================================================");
                                    response.Add("Snort Exiting");
                                    response.Add("");
                                }
                                else
                                {
                                    response.Add("ERROR: Command " + args[5] + " tidak ditemukan");
                                }
                            }
                            else
                            {
                                response.Add("ERROR: Command " + args[4] + " tidak ditemukan");
                            }
                        }
                        else
                        {
                            response.Add("ERROR: Command " + args[3] + " tidak ditemukan");
                        }

                    }
                    else if (args[2] == "-r")
                    {
                        if (args.Length == 3)
                        {
                            response.Add("ERROR: Command tidak ditemukan");
                        }
                        else if (args[3] == "/var/log/snort/snort.log.716")
                        {
                            packetSnort2 = false;
                            SaveInstalledTools();
                            OutputSnort();
                        }
                        else
                        {
                            response.Add("ERROR: Command " + args[3] + " tidak ditemukan");
                        }
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                    }
                }
                else
                {
                    response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
                }
            }
        }
        else
        {
            InputUmum();
        }
    }

    //done
    private void OutputSnort()
    {
        response.Add("Running in packet logging mode");
        response.Add("");
        response.Add("        --== Initializing Snort ==--");
        response.Add("Initializing Output Plugins!");
        response.Add("***");
        response.Add("*** interface device lookup found: eth0");
        response.Add("***");
        response.Add("Initializing Network Interface eth0");
        response.Add("Decoding Ethernet on interface eth0");
        response.Add("");
        response.Add("        --== Initialization Complete ==--");
        response.Add("");
        response.Add("   ,,_     -*> Snort! <*-");
        response.Add("  o\")~Version 2.8.5.2(Build 121) ");
        response.Add("   ''''    By Martin Roesch & The Snort Team: http://www.snort.org/contact#team");
        response.Add("           Copyright (C) 1998-2009 Sourcefire, Inc., et al.");
        response.Add("           Using PCRE version: 8.02 2010-03-19");
        response.Add("");
        response.Add("Not Using PCAP_FRAMES");
        response.Add("05/11-18:24:10.845870 0:C:29:98:21:43 -> 0:C:29:E:CD:86 type:0x800 len:0x62");
        response.Add("192.168.10.20 -> 192.168.10.10 ICMP TTL:64 TOS:0X0 ID:56069 IpLen:20 DgmLen:84 DF");
        response.Add("Type:8 Code:0 ID:1709 Seq:95 ECHO");
        response.Add("02 F0 B8 9E B6 7F 02 C8 85 B5 5A AA 08 00 45 00  ..........Z...E.");
        response.Add("00 54 89 DA 40 00 40 06 99 0A 0A 64 01 CA 0A 0A  .T..@.@....d....");
        response.Add("01 88 84 D4 00 50 B7 CC C0 31 21 CE B6 7C 80 18  .....P...1!..|..");
        response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
        response.Add("");
        response.Add("05/11-18:24:10.845891 0:C:29:E:CD:86 -> 0:C:29:98:21:43 type:0x800 len:0x62");
        response.Add("192.168.10.10 -> 192.168.10.20 ICMP TTL:64 TOS:0X0 ID:53279 IpLen:20 DgmLen:84");
        response.Add("Type:0 Code:0 ID:1709 Seq:95 ECHO REPLY");
        response.Add("02 F0 B8 9E B6 7F 02 C8 85 B5 5A AA 08 00 45 00  ..........Z...E.");
        response.Add("00 54 89 DA 40 00 40 06 99 0A 0A 64 01 CA 0A 0A  .T..@.@....d....");
        response.Add("01 88 84 D4 00 50 B7 CC C0 31 21 CE B6 7C 80 18  .....P...1!..|..");
        response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
        response.Add("");
        response.Add("05/11-18:24:11.854801 0:C:29:98:21:43 -> 0:C:29:E:CD:86 type:0x800 len:0x62");
        response.Add("192.168.10.20 -> 192.168.10.10 ICMP TTL:64 TOS:0X0 ID:57018 IpLen:20 DgmLen:84 DF");
        response.Add("Type:8 Code:0 ID:1709 Seq:96 ECHO");
        response.Add("02 F0 B8 9E B6 7F 02 C8 85 B5 5A AA 08 00 45 00  ..........Z...E.");
        response.Add("00 54 89 DA 40 00 40 06 99 0A 0A 64 01 CA 0A 0A  .T..@.@....d....");
        response.Add("01 88 84 D4 00 50 B7 CC C0 31 21 CE B6 7C 80 18  .....P...1!..|..");
        response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
        response.Add("");
        response.Add("05/11-18:24:10.856518 0:C:29:E:CD:86 -> 0:C:29:98:21:43 type:0x800 len:0x62");
        response.Add("192.168.10.10 -> 192.168.10.20 ICMP TTL:64 TOS:0X0 ID:54217 IpLen:20 DgmLen:84");
        response.Add("Type:0 Code:0 ID:1709 Seq:96 ECHO REPLY");
        response.Add("02 F0 B8 9E B6 7F 02 C8 85 B5 5A AA 08 00 45 00  ..........Z...E.");
        response.Add("00 54 89 DA 40 00 40 06 99 0A 0A 64 01 CA 0A 0A  .T..@.@....d....");
        response.Add("01 88 84 D4 00 50 B7 CC C0 31 21 CE B6 7C 80 18  .....P...1!..|..");
        response.Add("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
        response.Add("Action Stats:");
        response.Add("ALERTS: 0");
        response.Add("LOGGED: 0");
        response.Add("PASSED: 0");
        response.Add("===========================================================");
        response.Add("Snort Exiting");
        response.Add("");
    }

    //done
    private void QuestNMAP()
    {
        if (args[0] == "apt-get")
        {
            if (args.Length == 1)
            {
                response.Add("ERROR: Command " + args[0] + " tidak memiliki parameter");
            }
            else if (args[1] == "install")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Tools belum dimasukkan");
                }
                else if (args[2] == "nmap")
                {
                    response.Add("Reading package lists… Done");
                    response.Add("Building dependency tree");
                    response.Add("Reading state information… Done");
                    response.Add("nmap is already the newest version.");
                    response.Add("0 upgraded, 0 newly installed, 0 to remove and 0 not upgraded.");
                    response.Add("");
                    nmapInstalled = true;
                    SaveInstalledTools();
                    Debug.Log("aircrack " + nmapInstalled);
                }
                else
                {
                    response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "ping")
        {
            if (args.Length == 1)
            {
                response.Add("Format: ping [ip]");
                response.Add("");
            }
            else if (args[1] == "173.101.10.10")
            {
                response.Add("PING " + args[1] + " 56(84) bytes of data.");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("");
                response.Add("--- " + args[1] + " ping statistics ---");
                response.Add("4 packets transmitted, 4 received, 0 % packet loss, time 5113ms");
                response.Add("rtt min / avg / max / mdev = 0.043 / 0.060 / 0.068 / 0.008 ms");
                response.Add("");
            }
        }
        else if (args[0] == "nmap" && nmapInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format:");
                response.Add("nmap [ip] \t\t\t\t Melakukan scanning sederhana");
                response.Add("nmap -sV [ip] \t\t\t Melakukan scanning version yang digunakan");
                response.Add("nmap -p [port] [ip] \t\t\t Melakukan scanning pada port tertentu");
                response.Add("nmap [ip network]/[prefix] \t\t Melakukan scanning pada satu jaringan");
                response.Add("nmap -O [ip] \t\t\t Mengecek OS yang digunakan target");
                response.Add("nmap -sn [ip network]/[prefix] \t Melakukan scanning network");
                response.Add("");
            }
            else if (args.Length == 2)
            {
                if (args[1] == "173.101.10.10")
                {
                    response.Add("Starting Nmap 7.94SVN ( https://nmap.org ) at 2024-04-16 06:17 EDT");
                    response.Add("Nmap scan report for 173.101.10.10");
                    response.Add("Host is up (0.24s latency).");
                    response.Add("Not shown: 998 closed tcp ports (conn-refused)");
                    response.Add("PORT      STATE    SERVICE");
                    response.Add("22/tcp    open     ssh");
                    response.Add("80/tcp    open     http");
                    response.Add("");
                    response.Add("Nmap done: 1 IP address (1 host up) scanned in 32.90 seconds");
                    response.Add("");
                }
                else if (args[1] == "173.101.10.0/24")
                {
                    response.Add("Starting Nmap 7.94SVN ( https://nmap.org ) at 2024-04-16 06:20 EDT");
                    response.Add("Nmap scan report for 173.101.10.1");
                    response.Add("Host is up (0.0018s latency).");
                    response.Add("Not shown: 999 closed tcp ports (conn-refused)");
                    response.Add("PORT      STATE    SERVICE");
                    response.Add("22/tcp    open     ssh");
                    response.Add("");
                    response.Add("Nmap scan report for 173.101.10.10");
                    response.Add("Host is up (0.24s latency).");
                    response.Add("Not shown: 998 closed tcp ports (conn-refused)");
                    response.Add("PORT      STATE    SERVICE");
                    response.Add("22/tcp    open     ssh");
                    response.Add("80/tcp    open     http");
                    response.Add("");
                    response.Add("Nmap scan report for 173.101.10.98");
                    response.Add("Host is up (0.00059s latency).");
                    response.Add("Not shown: 999 closed tcp ports (conn-refused)");
                    response.Add("PORT      STATE    SERVICE");
                    response.Add("80/tcp    open     http");
                    response.Add("");
                    response.Add("Nmap done: 256 IP addresses (3 hosts up) scanned in 10.65 seconds");
                    response.Add("");
                }
                else
                {
                    response.Add("ERROR: Ip " + args[1] + " tidak ditemukan");
                }
            }
            else if (args.Length > 2)
            {
                if (args[1] == "-sV")
                {
                    if (args.Length == 2)
                    {
                        response.Add("ERROR: Command tidak ditemukan");
                    }
                    else if (args[2] == "173.101.10.10")
                    {
                        response.Add("Starting Nmap 7.94SVN ( https://nmap.org ) at 2024-04-16 06:19 EDT");
                        response.Add("Nmap scan report for 173.101.10.10");
                        response.Add("Host is up (0.23s latency).");
                        response.Add("Not shown: 998 closed tcp ports (conn-refused)");
                        response.Add("PORT      STATE    SERVICE    VERSION");
                        response.Add("22/tcp    open     ssh        ssh123");
                        response.Add("80/tcp    open     http       http223");
                        response.Add("");
                        response.Add("Nmap done: 1 IP address (1 host up) scanned in 36.12 seconds");
                        response.Add("");
                    }
                    else
                    {
                        response.Add("ERROR: Ip " + args[2] + " tidak ditemukan");
                    }
                }
                else if (args[1] == "-p")
                {
                    if (args.Length == 2)
                    {
                        response.Add("ERROR: Command tidak ditemukan");
                    }
                    else if (args[2] == "22")
                    {
                        if (args.Length == 3)
                        {
                            response.Add("ERROR: Command tidak ditemukan");
                        }
                        else if (args[3] == "173.101.10.10")
                        {
                            response.Add("Starting Nmap 7.94SVN ( https://nmap.org ) at 2024-04-16 06:20 EDT");
                            response.Add("Nmap scan report for 173.101.10.10");
                            response.Add("Host is up (0.25s latency).");
                            response.Add("");
                            response.Add("PORT      STATE    SERVICE");
                            response.Add("22/tcp    open     ssh");
                            response.Add("");
                            response.Add("Nmap done: 1 IP address (1 host up) scanned in 0.55 seconds");
                            response.Add("");
                        }
                        else
                        {
                            response.Add("ERROR: Ip " + args[3] + " tidak ditemukan");
                        }
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                    }
                }
                else
                {
                    response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
                }
            }
        }
        else
        {
            InputUmum();
        }
    }

    //done
    private void QuestNMAPScanIP()
    {
        if (args[0] == "ping")
        {
            if (args.Length == 1)
            {
                response.Add("Format: ping [ip]");
                response.Add("");
            }
            else if (args[1] == "213.91.32.9")
            {
                response.Add("PING " + args[1] + " 56(84) bytes of data.");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("");
                response.Add("--- " + args[1] + " ping statistics ---");
                response.Add("4 packets transmitted, 4 received, 0 % packet loss, time 5113ms");
                response.Add("rtt min / avg / max / mdev = 0.043 / 0.060 / 0.068 / 0.008 ms");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Ip " + args[1] + " tidak ditemukan");
            }

        }
        else if (args[0] == "nmap" && nmapInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format:");
                response.Add("nmap [ip] \t\t\t\t Melakukan scanning sederhana");
                response.Add("nmap -sV [ip] \t\t\t Melakukan scanning version yang digunakan");
                response.Add("nmap -p [port] [ip] \t\t\t Melakukan scanning pada port tertentu");
                response.Add("nmap [ip network]/[prefix] \t\t Melakukan scanning pada satu jaringan");
                response.Add("nmap -O [ip] \t\t\t Mengecek OS yang digunakan target");
                response.Add("nmap -sn [ip network]/[prefix] \t Melakukan scanning network");
                response.Add("");
            }
            else if (args[1] == "213.91.32.9")
            {
                response.Add("Starting Nmap 7.94SVN ( https://nmap.org ) at 2024-04-16 06:17 EDT");
                response.Add("Nmap scan report for 213.91.32.9");
                response.Add("Host is up (0.24s latency).");
                response.Add("Not shown: 999 closed tcp ports (conn-refused)");
                response.Add("PORT      STATE    SERVICE");
                response.Add("22/tcp    open     ssh");
                response.Add("");
                response.Add("Nmap done: 1 IP address (1 host up) scanned in 32.90 seconds");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Ip " + args[1] + " tidak ditemukan");
            }
        }
        else
        {
            InputUmum();
        }
    }

    //done
    private void QuestNMAPScanPort()
    {
        if (args[0] == "ping")
        {
            if (args.Length == 1)
            {
                response.Add("Format: ping [ip]");
                response.Add("");
            }
            else if (args[1] == "241.13.1.18")
            {
                response.Add("PING " + args[1] + " 56(84) bytes of data.");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("");
                response.Add("--- " + args[1] + " ping statistics ---");
                response.Add("4 packets transmitted, 4 received, 0 % packet loss, time 5113ms");
                response.Add("rtt min / avg / max / mdev = 0.043 / 0.060 / 0.068 / 0.008 ms");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Ip " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "nmap" && nmapInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format:");
                response.Add("nmap [ip] \t\t\t\t Melakukan scanning sederhana");
                response.Add("nmap -sV [ip] \t\t\t Melakukan scanning version yang digunakan");
                response.Add("nmap -p [port] [ip] \t\t\t Melakukan scanning pada port tertentu");
                response.Add("nmap [ip network]/[prefix] \t\t Melakukan scanning pada satu jaringan");
                response.Add("nmap -O [ip] \t\t\t Mengecek OS yang digunakan target");
                response.Add("nmap -sn [ip network]/[prefix] \t Melakukan scanning network");
                response.Add("");
            }
            else if (args[1] == "-p")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Command tidak ditemukan");
                }
                else if (args[2] == "80")
                {
                    if (args.Length == 3)
                    {
                        response.Add("ERROR: Command tidak ditemukan");
                    }
                    else if (args[3] == "241.13.1.18")
                    {
                        response.Add("Starting Nmap 7.94SVN ( https://nmap.org ) at 2024-04-16 06:20 EDT");
                        response.Add("Nmap scan report for 241.13.1.18");
                        response.Add("Host is up (0.25s latency).");
                        response.Add("");
                        response.Add("PORT      STATE    SERVICE");
                        response.Add("80/tcp    open     http");
                        response.Add("");
                        response.Add("Nmap done: 1 IP address (1 host up) scanned in 0.55 seconds");
                        response.Add("");
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[3] + " tidak ditemukan");
                    }
                }
                else
                {
                    response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else
        {
            InputUmum();
        }
    }

    //done
    private void QuestIntroExploit()
    {
        if (args[0] == "ping")
        {
            if (args.Length == 1)
            {
                response.Add("Format: ping [ip]");
                response.Add("");
            }
            else if (args[1] == "203.128.56.77")
            {
                response.Add("PING " + args[1] + " 56(84) bytes of data.");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("");
                response.Add("--- " + args[1] + " ping statistics ---");
                response.Add("4 packets transmitted, 4 received, 0 % packet loss, time 5113ms");
                response.Add("rtt min / avg / max / mdev = 0.043 / 0.060 / 0.068 / 0.008 ms");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Ip " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "nmap" && nmapInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format:");
                response.Add("nmap [ip] \t\t\t\t Melakukan scanning sederhana");
                response.Add("nmap -sV [ip] \t\t\t Melakukan scanning version yang digunakan");
                response.Add("nmap -p [port] [ip] \t\t\t Melakukan scanning pada port tertentu");
                response.Add("nmap [ip network]/[prefix] \t\t Melakukan scanning pada satu jaringan");
                response.Add("nmap -O [ip] \t\t\t Mengecek OS yang digunakan target");
                response.Add("nmap -sn [ip network]/[prefix] \t Melakukan scanning network");
                response.Add("");
            }
            else if (args[1] == "-sV")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Target belum dimasukkan");
                }
                if (args[2] == "203.128.56.77")
                {
                    //input benar
                    response.Add("Starting Nmap 7.94SVN ( https://nmap.org ) at 2024-04-16 06:19 EDT");
                    response.Add("Nmap scan report for 203.128.56.77");
                    response.Add("Host is up (0.23s latency).");
                    response.Add("Not shown: 999 closed tcp ports (conn-refused)");
                    response.Add("PORT      STATE    SERVICE    VERSION");
                    response.Add("80/tcp    open     http       1.1.8");
                    response.Add("");
                    response.Add("Nmap done: 1 IP address (1 host up) scanned in 36.12 seconds");
                    response.Add("");
                }
                else
                {
                    response.Add("ERROR: Ip " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "python3")
        {
            if (args.Length == 1)
            {
                response.Add("Format: python3 [nama folder]/[nama file exploit] [ip target]");
                response.Add("");
            }
            else if (args[1] == "download/http118.py")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: IP target belum dimasukkan");
                }
                else if (args[2] == "203.128.56.77")
                {
                    TerminalManager.meterpreter = true;
                    SaveInstalledTools();
                    response.Add("[+] Started reverse TCP handler");
                    response.Add("[+] 203.128.56.77 - Generating payload . . .");
                    response.Add("[+] Sending stage (716,271 bytes) to 203.128.56.77");
                    response.Add("[+] Meterpreter session opened (-> 203.128.56.77)");
                }
                else
                {
                    response.Add("ERROR: Ip " + args[2] + " tidak ditemukan");
                }

            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "ls" && TerminalManager.meterpreter)
        {
            if (args.Length == 1)
            {
                response.Add("downloads");
                response.Add("file");
                response.Add("pictures");
                response.Add("movies");
                response.Add("");
            }
            else if (args[1] == "downloads")
            {
                response.Add("File tidak ditemukan");
                response.Add("");
            }
            else if (args[1] == "file")
            {
                response.Add("report.txt");
                response.Add("");
            }
            else if (args[1] == "pictures")
            {
                response.Add("home.txt");
                response.Add("");
            }
            else if (args[1] == "movies")
            {
                response.Add("movie123.txt");
                response.Add("hellopanda.txt");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "cat" && TerminalManager.meterpreter)
        {
            if (args.Length == 1)
            {
                response.Add("Format: cat [folder]/[nama file]");
            }
            else if (args[1] == "file/report.txt")
            {
                response.Add("081 888 999 716");
                response.Add("");
            }
            else if (args[1] == "pictures/home.txt")
            {
                response.Add("Jl. Manggis No. 178");
                response.Add("");
            }
            else if (args[1] == "movies/movie123.txt")
            {
                response.Add("This is my movie");
                response.Add("");
            }
            else if (args[1] == "movies/hellopanda.txt")
            {
                response.Add("This is hello panda movie");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }

        }
        else if (args[0] == "download" && TerminalManager.meterpreter)
        {
            if (args[1] == "file/report.txt")
            {
                response.Add("Downloading file \"report.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 1;
                SaveInstalledTools();
            }
            // this
            else if (args[1] == "pictures/home.txt")
            {
                response.Add("Downloading file \"home.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 2;
                SaveInstalledTools();
            }
            else if (args[1] == "movies/movie123.txt")
            {
                response.Add("Downloading file \"movie123.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 3;
                SaveInstalledTools();
            }
            else if (args[1] == "movies/hellopanda.txt")
            {
                response.Add("Downloading file \"hellopanda.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 4;
                SaveInstalledTools();
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "exit")
        {
            TerminalManager.meterpreter = false;
            SaveInstalledTools();
        }
        else
        {
            InputUmum();
        }
    }

    //done
    private void QuestExploit1()
    {
        //fileDownload = 0;

        if (args[0] == "ping")
        {
            if (args.Length == 1)
            {
                response.Add("Format: ping [ip]");
                response.Add("");
            }
            else if (args[1] == "92.201.143.234")
            {
                response.Add("PING " + args[1] + " 56(84) bytes of data.");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("");
                response.Add("--- " + args[1] + " ping statistics ---");
                response.Add("4 packets transmitted, 4 received, 0 % packet loss, time 5113ms");
                response.Add("rtt min / avg / max / mdev = 0.043 / 0.060 / 0.068 / 0.008 ms");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Ip " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "nmap" && nmapInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format:");
                response.Add("nmap [ip] \t\t\t\t Melakukan scanning sederhana");
                response.Add("nmap -sV [ip] \t\t\t Melakukan scanning version yang digunakan");
                response.Add("nmap -p [port] [ip] \t\t\t Melakukan scanning pada port tertentu");
                response.Add("nmap [ip network]/[prefix] \t\t Melakukan scanning pada satu jaringan");
                response.Add("nmap -O [ip] \t\t\t Mengecek OS yang digunakan target");
                response.Add("nmap -sn [ip network]/[prefix] \t Melakukan scanning network");
                response.Add("");
            }
            else if (args[1] == "-sV")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Target belum dimasukkan");
                }
                if (args[2] == "92.201.143.234")
                {
                    //input benar
                    response.Add("Starting Nmap 7.94SVN ( https://nmap.org ) at 2024-04-16 06:19 EDT");
                    response.Add("Nmap scan report for 92.201.143.234");
                    response.Add("Host is up (0.23s latency).");
                    response.Add("Not shown: 999 closed tcp ports (conn-refused)");
                    response.Add("PORT      STATE    SERVICE    VERSION");
                    response.Add("80/tcp    open     http       1.1.6");
                    response.Add("");
                    response.Add("Nmap done: 1 IP address (1 host up) scanned in 36.12 seconds");
                    response.Add("");
                }
                else
                {
                    response.Add("ERROR: Ip " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "python3")
        {
            if (args.Length == 1)
            {
                response.Add("Format: python3 [nama folder]/[nama file exploit] [ip target]");
                response.Add("");
            }
            else if (args[1] == "download/http116.py")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: IP target belum dimasukkan");
                }
                else if (args[2] == "92.201.143.234")
                {
                    TerminalManager.meterpreter = true;
                    SaveInstalledTools();
                    response.Add("[+] Started reverse TCP handler");
                    response.Add("[+] 92.201.143.234 - Generating payload . . .");
                    response.Add("[+] Sending stage (716,271 bytes) to 92.201.143.234");
                    response.Add("[+] Meterpreter session opened (-> 92.201.143.234)");
                }
                else
                {
                    response.Add("ERROR: Ip " + args[2] + " tidak ditemukan");
                }

            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "ls" && TerminalManager.meterpreter)
        {
            if (args.Length == 1)
            {
                response.Add("downloads");
                response.Add("memories");
                response.Add("mytask");
                response.Add("videos");
                response.Add("");
            }
            else if (args[1] == "downloads")
            {
                response.Add("my.txt");
                response.Add("");
            }
            else if (args[1] == "memories")
            {
                response.Add("File tidak ditemukan");
                response.Add("");
            }
            else if (args[1] == "mytask")
            {
                response.Add("task1.txt");
                response.Add("task2.txt");
                response.Add("");
            }
            else if (args[1] == "videos")
            {
                response.Add("take1.txt");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "cat" && TerminalManager.meterpreter)
        {
            if (args.Length == 1)
            {
                response.Add("Format: cat [folder]/[nama file]");
            }
            else if (args[1] == "downloads/my.txt")
            {
                response.Add("081 819 467 102");
                response.Add("");
            }
            else if (args[1] == "mytask/task1.txt")
            {
                response.Add("Kerjakan task ini sekarang juga");
                response.Add("");
            }
            else if (args[1] == "mytask/task2.txt")
            {
                response.Add("Lanjut kerjakan task 2");
                response.Add("");
            }
            else if (args[1] == "videos/take1.txt")
            {
                response.Add("This is my first video ever");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "download" && TerminalManager.meterpreter)
        {
            // this
            if (args[1] == "downloads/my.txt")
            {
                response.Add("Downloading file \"my.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 5;
                SaveInstalledTools();
            }
            else if (args[1] == "mytask/task1.txt")
            {
                response.Add("Downloading file \"task1.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 6;
                SaveInstalledTools();
            }
            else if (args[1] == "mytask/task2.txt")
            {
                response.Add("Downloading file \"task2.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 7;
                SaveInstalledTools();
            }
            else if (args[1] == "videos/take1.txt")
            {
                response.Add("Downloading file \"take1.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 8;
                SaveInstalledTools();
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "exit")
        {
            TerminalManager.meterpreter = false;
            SaveInstalledTools();
        }
        else
        {
            InputUmum();
        }
    }

    //done
    private void QuestExploit2()
    {
        //fileDownload = 0;

        if (args[0] == "ping")
        {
            if (args.Length == 1)
            {
                response.Add("Format: ping [ip]");
                response.Add("");
            }
            else if (args[1] == "175.92.18.109")
            {
                response.Add("PING " + args[1] + " 56(84) bytes of data.");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("");
                response.Add("--- " + args[1] + " ping statistics ---");
                response.Add("4 packets transmitted, 4 received, 0 % packet loss, time 5113ms");
                response.Add("rtt min / avg / max / mdev = 0.043 / 0.060 / 0.068 / 0.008 ms");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Ip " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "nmap" && nmapInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format:");
                response.Add("nmap [ip] \t\t\t\t Melakukan scanning sederhana");
                response.Add("nmap -sV [ip] \t\t\t Melakukan scanning version yang digunakan");
                response.Add("nmap -p [port] [ip] \t\t\t Melakukan scanning pada port tertentu");
                response.Add("nmap [ip network]/[prefix] \t\t Melakukan scanning pada satu jaringan");
                response.Add("nmap -O [ip] \t\t\t Mengecek OS yang digunakan target");
                response.Add("nmap -sn [ip network]/[prefix] \t Melakukan scanning network");
                response.Add("");
            }
            else if (args[1] == "-sV")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Target belum dimasukkan");
                }
                if (args[2] == "175.92.18.109")
                {
                    //input benar
                    response.Add("Starting Nmap 7.94SVN ( https://nmap.org ) at 2024-04-16 06:19 EDT");
                    response.Add("Nmap scan report for 175.92.18.109");
                    response.Add("Host is up (0.23s latency).");
                    response.Add("Not shown: 999 closed tcp ports (conn-refused)");
                    response.Add("PORT      STATE    SERVICE    VERSION");
                    response.Add("80/tcp    open     http       1.1.3");
                    response.Add("");
                    response.Add("Nmap done: 1 IP address (1 host up) scanned in 36.12 seconds");
                    response.Add("");
                }
                else
                {
                    response.Add("ERROR: Ip " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "python3")
        {
            if (args.Length == 1)
            {
                response.Add("Format: python3 [nama folder]/[nama file exploit] [ip target]");
                response.Add("");
            }
            else if (args[1] == "download/http113.py")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: IP target belum dimasukkan");
                }
                else if (args[2] == "175.92.18.109")
                {
                    TerminalManager.meterpreter = true;
                    SaveInstalledTools();
                    response.Add("[+] Started reverse TCP handler");
                    response.Add("[+] 175.92.18.109 - Generating payload . . .");
                    response.Add("[+] Sending stage (716,271 bytes) to 175.92.18.109");
                    response.Add("[+] Meterpreter session opened (-> 175.92.18.109)");
                }
                else
                {
                    response.Add("ERROR: Ip " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "ls" && TerminalManager.meterpreter)
        {
            if (args.Length == 1)
            {
                response.Add("documents");
                response.Add("music");
                response.Add("recently");
                response.Add("videos");
                response.Add("");
            }
            else if (args[1] == "documents")
            {
                response.Add("File tidak ditemukan");
                response.Add("");
            }
            else if (args[1] == "music")
            {
                response.Add("record.txt");
                response.Add("");
            }
            else if (args[1] == "recently")
            {
                response.Add("music1.txt");
                response.Add("");
            }
            else if (args[1] == "videos")
            {
                response.Add("account.txt");
                response.Add("bank.txt");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "cat" && TerminalManager.meterpreter)
        {
            if (args.Length == 1)
            {
                response.Add("Format: cat [folder]/[nama file]");
            }
            else if (args[1] == "music/record.txt")
            {
                response.Add("Ini file record music yang pertama");
                response.Add("");
            }
            else if (args[1] == "recently/music1.txt")
            {
                response.Add("Bigbang - bang bang bang");
                response.Add("");
            }
            else if (args[1] == "videos/account.txt")
            {
                response.Add("8917 7391 7283 1649");
                response.Add("");
            }
            else if (args[1] == "videos/bank.txt")
            {
                response.Add("082 173 827 163");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "download" && TerminalManager.meterpreter)
        {
            if (args[1] == "music/record.txt")
            {
                response.Add("Downloading file \"record.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 9;
                SaveInstalledTools();
            }
            else if (args[1] == "recently/music1.txt")
            {
                response.Add("Downloading file \"music1.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 10;
                SaveInstalledTools();
            }
            // this
            else if (args[1] == "videos/account.txt")
            {
                response.Add("Downloading file \"account.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 11;
                SaveInstalledTools();
            }
            else if (args[1] == "videos/bank.txt")
            {
                response.Add("Downloading file \"bank.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 12;
                SaveInstalledTools();
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "exit")
        {
            TerminalManager.meterpreter = false;
            SaveInstalledTools();
        }
        else
        {
            InputUmum();
        }
    }

    //done
    private void QuestOSIntro()
    {
        fileDownload = 0;

        if (args[0] == "ping")
        {
            if (args.Length == 1)
            {
                response.Add("Format: ping [ip]");
                response.Add("");
            }
            else if (args[1] == "109.47.215.86")
            {
                response.Add("PING " + args[1] + " 56(84) bytes of data.");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("");
                response.Add("--- " + args[1] + " ping statistics ---");
                response.Add("4 packets transmitted, 4 received, 0 % packet loss, time 5113ms");
                response.Add("rtt min / avg / max / mdev = 0.043 / 0.060 / 0.068 / 0.008 ms");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Ip " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "nmap" && nmapInstalled)
        {
            if (args.Length == 1)
            {
                //format nmap
                response.Add("Format:");
                response.Add("nmap [ip] \t\t\t\t Melakukan scanning sederhana");
                response.Add("nmap -sV [ip] \t\t\t Melakukan scanning version yang digunakan");
                response.Add("nmap -p [port] [ip] \t\t\t Melakukan scanning pada port tertentu");
                response.Add("nmap [ip network]/[prefix] \t\t Melakukan scanning pada satu jaringan");
                response.Add("nmap -O [ip] \t\t\t Mengecek OS yang digunakan target");
                response.Add("nmap -sn [ip network]/[prefix] \t Melakukan scanning network");
                response.Add("");
            }
            else if (args[1] == "-O")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Command tidak ditemukan");
                }
                else if (args[2] == "109.47.215.86")
                {
                    //input benar
                    response.Add("Starting Nmap 7.80 ( https://nmap.org ) at 2024-05-01 15:10 EDT");
                    response.Add("Nmap scan report for 109.47.215.86");
                    response.Add("Host is up (0.23s latency).");
                    response.Add("Not shown: 997 filtered ports");
                    response.Add("PORT      STATE    SERVICE");
                    response.Add("80/tcp    open     http");
                    response.Add("443/tcp   open     https");
                    response.Add("3389/tcp  open     ms-wbt-server");
                    response.Add("");
                    response.Add("Device type: general purpose");
                    response.Add("Running: Linux 3.X|4.X");
                    response.Add("OS CPE: cpe:/o:linux:linux_kernel:3 cpe:/o:linux:linux_kernel:4");
                    response.Add("OS details: Linux 3.2 - 4.9");
                    response.Add("Network Distance: 13 hops");
                    response.Add("");
                    response.Add("OS detection performed. Please report any incorrect results at https://nmap.org/submit/ .");
                    response.Add("Nmap done: 1 IP address (1 host up) scanned in 32.90 seconds");
                    response.Add("");
                }
                else
                {
                    response.Add("ERROR: Ip " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else
        {
            InputUmum();
        }
    }

    //done
    private void QuestOS()
    {
        if (args[0] == "ping")
        {
            if (args.Length == 1)
            {
                response.Add("Format: ping [ip]");
                response.Add("");
            }
            else if (args[1] == "184.72.33.210")
            {
                response.Add("PING " + args[1] + " 56(84) bytes of data.");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("64 bytes from " + args[1] + ": icmp_seq = 1 ttl = 64 time = 0.043 ms");
                response.Add("");
                response.Add("--- " + args[1] + " ping statistics ---");
                response.Add("4 packets transmitted, 4 received, 0 % packet loss, time 5113ms");
                response.Add("rtt min / avg / max / mdev = 0.043 / 0.060 / 0.068 / 0.008 ms");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Ip " + args[1] + " tidak ditemukan");
            }

        }
        else if (args[0] == "nmap" && nmapInstalled)
        {
            if (args.Length == 1)
            {
                //format nmap
                response.Add("Format:");
                response.Add("nmap [ip] \t\t\t\t Melakukan scanning sederhana");
                response.Add("nmap -sV [ip] \t\t\t Melakukan scanning version yang digunakan");
                response.Add("nmap -p [port] [ip] \t\t\t Melakukan scanning pada port tertentu");
                response.Add("nmap [ip network]/[prefix] \t\t Melakukan scanning pada satu jaringan");
                response.Add("nmap -O [ip] \t\t\t Mengecek OS yang digunakan target");
                response.Add("nmap -sn [ip network]/[prefix] \t Melakukan scanning network");
                response.Add("");
            }
            else if (args[1] == "-O")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Command tidak ditemukan");
                }
                if (args[2] == "184.72.33.210")
                {
                    //input benar
                    response.Add("Starting Nmap 7.80 ( https://nmap.org ) at 2024-05-01 15:10 EDT");
                    response.Add("Nmap scan report for 184.72.33.210");
                    response.Add("Host is up (0.0023s latency).");
                    response.Add("Not shown: 997 filtered ports");
                    response.Add("PORT      STATE    SERVICE");
                    response.Add("22/tcp    open     ssh");
                    response.Add("80/tcp    open     http");
                    response.Add("443/tcp   open     https");
                    response.Add("");
                    response.Add("Device type: general purpose");
                    response.Add("Running: Linux 2.6.X");
                    response.Add("OS CPE: cpe:/o:linux:linux_kernel:2.6");
                    response.Add("OS details: Linux 2.6.32 - 2.6.39");
                    response.Add("Network Distance: 1 hops");
                    response.Add("");
                    response.Add("OS detection performed. Please report any incorrect results at https://nmap.org/submit/ .");
                    response.Add("Nmap done: 1 IP address (1 host up) scanned in 55.19 seconds");
                    response.Add("");
                }
                else
                {
                    response.Add("ERROR: Ip " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else
        {
            InputUmum();
        }
    }

    // done
    private void QuestNS()
    {
        if (args[0] == "nmap" && nmapInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format:");
                response.Add("nmap [ip] \t\t\t\t Melakukan scanning sederhana");
                response.Add("nmap -sV [ip] \t\t\t Melakukan scanning version yang digunakan");
                response.Add("nmap -p [port] [ip] \t\t\t Melakukan scanning pada port tertentu");
                response.Add("nmap [ip network]/[prefix] \t\t Melakukan scanning pada satu jaringan");
                response.Add("nmap -O [ip] \t\t\t Mengecek OS yang digunakan target");
                response.Add("nmap -sn [ip network]/[prefix] \t Melakukan scanning network");
                response.Add("");
            }
            else if (args[1] == "-sn")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Target belum dimasukkan");
                }
                if (args[2] == "192.168.43.0/24")
                {
                    //input benar
                    response.Add("Starting Nmap 7.94SVN ( https://nmap.org ) at 2024-01-27 23:01 EST");
                    response.Add("Nmap scan report for 192.168.43.1");
                    response.Add("Host is up (0.0040s latency).");
                    response.Add("Nmap scan report for 192.168.43.2");
                    response.Add("Host is up (0.0053s latency).");
                    response.Add("Nmap scan report for 192.168.43.43");
                    response.Add("Host is up (0.0059s latency).");
                    response.Add("Nmap scan report for 192.168.43.61");
                    response.Add("Host is up (0.0042s latency).");
                    response.Add("Nmap scan report for 192.168.43.87");
                    response.Add("Host is up (0.0055s latency).");
                    response.Add("Nmap scan report for 192.168.43.96");
                    response.Add("Host is up (0.0032s latency).");
                    response.Add("Nmap done: 256 IP addresses (6 hosts up) scanned in 5.22 seconds");
                    response.Add("");
                }
                else
                {
                    response.Add("ERROR: Ip " + args[2] + " tidak ditemukan");
                }

            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "iwconfig")
        {
            if (isModeMonitor)
            {
                // Jika status mode adalah monitor, kirim pesan mode monitor
                response.Add("Wlan0mon		IEEE 802.11 Mode:Monitor Frequency:2.457 Ghz Tx-Power=20 dBm");
                //StartCoroutine(Loading(1f));
                response.Add("\t\tRetry short limit:7 RTS thr:off Fragment thr:off");
                response.Add("\t\tPower Management:off");
                response.Add("");
            }
            else
            {
                // Jika status mode adalah managed, kirim pesan mode managed
                response.Add("Wlan0		IEEE 802.11 ESSID:off/any");
                response.Add("\t\tMode: Managed Access Point: Not - Associated Tx - Power = 20 dBm");
                response.Add("\t\tRetry short limit: 7 RTS thr: off Fragment thr: off");
                response.Add("\t\tEncryption key: off");
                response.Add("\t\tPower Management: off");
                response.Add("");
            }
        }
        else if (args[0] == "airmon-ng" && aircrackInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format: airmon-ng [start/stop] [interface]");
                response.Add("");
            }
            else if (args.Length > 1)
            {
                if (args[1] == "start")
                {
                    if (args.Length == 2)
                    {
                        response.Add("ERROR: Interface belum dimasukkan");
                    }
                    else if (args[2] == "wlan0")
                    {
                        // Set status mode menjadi monitor
                        isModeMonitor = true;
                        SaveInstalledTools();
                        Debug.Log("mode monitor " + isModeMonitor);
                        response.Add("PHY\tInterface\tDriver\tChipset");
                        response.Add("Phy0\twlan0\tath9k_htc\tQualcom Atheros Communications AR9271 802.11n");
                        response.Add("\t\t\t(mac80211 monitor mode vif enabled for [phy0]wlan0 on [phy0]wlan0mon)");
                        response.Add("\t\t\t(mac80211 station mode vif disabled for [phy0]wlan0)");
                        response.Add("");
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[2] + " tidak ditemukan"); ;
                    }
                }
                else if (args[1] == "stop")
                {
                    if (args.Length == 2)
                    {
                        response.Add("ERROR: Interface belum dimasukkan");
                    }
                    else if (args[2] == "wlan0mon")
                    {
                        // Set status mode menjadi monitor
                        isModeMonitor = false;
                        SaveInstalledTools();
                        Debug.Log("mode monitor " + isModeMonitor);
                        response.Add("PHY\tInterface\tDriver\tChipset");
                        response.Add("Phy0\twlan0\tath9k_htc\tQualcom Atheros Communications AR9271 802.11n");
                        response.Add("\t\t\t(mac80211 station mode vif enabled for [phy0]wlan0)");
                        response.Add("");
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[2] + " tidak ditemukan"); ;
                    }
                }
                else
                {
                    response.Add("ERROR: Command " + args[1] + " tidak ditemukan"); ;
                }
            }
        }
        else if (args[0] == "airodump-ng" && aircrackInstalled && isModeMonitor)
        {
            if (args.Length == 1)
            {
                response.Add("Format:");
                response.Add("Untuk memantau jaringan wifi:");
                response.Add("airodump-ng [interface]");
                response.Add("Untuk menangkap file handshake:");
                response.Add("airodump-ng -w [nama file handshake] -c [channel] -bssid [bssid]");
                response.Add("");
            }
            else if (args.Length == 2)
            {
                if (args[1] == "wlan0mon")
                {
                    response.Add(" CH 8 ][ Elapsed: 18 s ][ 2024-03-22 14:02");
                    response.Add("BSSID\t\tPWR\tBeacons\tCH\tMB\tENC\tCIPHER\tAUTH\tESSID");
                    response.Add(bssid1 + "\t-56\t16\t" + channel1 + "\t130\tWPA2\tCCMP\tPSK\tWifi ke satu");
                    response.Add(bssid2 + "\t-72\t10\t" + channel2 + "\t127\tWPA2\tCCMP\tPSK\tWifi ke dua");
                    response.Add("");
                    response.Add("BSSID\t\tSTATION\t\tPWR\tRate\tLost\tFrames\tNotes\tProbes");
                    response.Add("A2:15:91:B3:2D:FF\t12:B4:1A:8D:C6:54\t-86\t0-1\t0\t2");
                    response.Add("33:12:BA:15:81:AB\tBA:91:65:C1:BB:71\t-72\t0-1e\t0\t1");
                    response.Add("Quitting...");
                    response.Add("");
                }
                else
                {
                    response.Add("ERROR: Interface " + args[1] + " tidak ditemukan");
                }
            }
            else if (args.Length > 2)
            {
                if (args[1] == "--bssid")
                {
                    currentBssid = args[2];
                    SaveInstalledTools();
                    if (args[3] == "--channel")
                    {
                        if (int.TryParse(args[4], out currentChannel))
                        {
                            SaveInstalledTools();
                            if (args[5] == "wlan0mon")
                            {
                                if (currentBssid == bssid1 || currentBssid == bssid2)
                                {
                                    if (currentChannel == channel1 && currentBssid == bssid1)
                                    {
                                        response.Add("BSSID\t\tPWR\tBeacons\tCH\tMB\tENC\tCIPHER\tAUTH\tESSID");
                                        response.Add(bssid1 + "\t-56\t16\t" + channel1 + "\t130\tWPA2\tCCMP\tPSK\tWifi ke satu");
                                        response.Add("");
                                        response.Add("BSSID\t\tSTATION\t\tPWR\tRate\tLost\tFrames\tNotes\tProbes");
                                        response.Add(bssid1 + "\tBC:51:81:AC:45:91\t-56\t0-1e\t0\t3");
                                        response.Add(bssid1 + "\t45:19:20:A0:1C:54\t-92\t0-6e\t0\t1");
                                        response.Add("Quitting...");
                                        response.Add("");
                                    }
                                    else if (currentChannel == channel2 && currentBssid == bssid2)
                                    {
                                        response.Add("BSSID\t\tPWR\tBeacons\tCH\tMB\tENC\tCIPHER\tAUTH\tESSID");
                                        response.Add(bssid2 + "\t-72\t10\t" + channel2 + "\t127\tWPA2\tCCMP\tPSK\tWifi ke dua");
                                        response.Add("");
                                        response.Add("BSSID\t\tSTATION\t\tPWR\tRate\tLost\tFrames\tNotes\tProbes");
                                        response.Add(bssid2 + "\tBC:51:81:AC:45:91\t-56\t0-1e\t0\t3");
                                        response.Add(bssid2 + "\t45:19:20:A0:1C:54\t-92\t0-6e\t0\t1");
                                        response.Add("Quitting...");
                                        response.Add("");
                                    }
                                    else
                                    {
                                        response.Add("ERROR: Channel dan BSSID tidak cocok");
                                    }
                                }
                                else
                                {
                                    response.Add("ERROR: BSSID tidak ditemukan");
                                }
                            }
                            else
                            {
                                response.Add("ERROR: Command " + args[5] + " tidak ditemukan");
                            }
                        }
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[3] + " tidak ditemukan");
                    }
                }
                else
                {
                    response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
                }
            }
        }
        else if (args[0] == "aireplay-ng" && aircrackInstalled && isModeMonitor)
        {
            if (args.Length == 1)
            {
                response.Add("Format: aireplay-ng --deauth -0 -a [bssid] [interface]");
                response.Add("");
            }
            else if (args.Length > 1)
            {
                if (args[1] == "--deauth")
                {
                    if (args[2] == "0")
                    {
                        if (args[3] == "-a")
                        {
                            if (args[4] == currentBssid)
                            {
                                SaveInstalledTools();
                                if (args.Length < 6 || args[5] == null)
                                {
                                    response.Add("ERROR: Interface tidak ditemukan");
                                }
                                else if (args[5] == "wlan0mon")
                                {
                                    response.Add("14:18:43 Waiting for beacon frame (BSSID: " + currentBssid + ") on channel " + currentChannel);
                                    response.Add("NB: this attack is more effective when targeting");
                                    response.Add("a connected wireless client (-c <client’s mac>).");
                                    response.Add("14:18:43 Sending DeAuth (code 7) to broadcast – BSSID: [" + currentBssid + "]");
                                    response.Add("14:18:43 Sending DeAuth (code 7) to broadcast – BSSID: [" + currentBssid + "]");
                                    response.Add("14:18:43 Sending DeAuth (code 7) to broadcast – BSSID: [" + currentBssid + "]");
                                    response.Add("14:18:43 Sending DeAuth (code 7) to broadcast – BSSID: [" + currentBssid + "]");
                                    response.Add("14:18:43 Sending DeAuth (code 7) to broadcast – BSSID: [" + currentBssid + "]");
                                    response.Add("");
                                }
                                else
                                {
                                    response.Add("ERROR: Interface " + args[5] + " tidak ditemukan");
                                }
                            }
                            else
                            {
                                response.Add("ERROR : BSSID yang dimasukkan salah");
                            }
                        }
                        else
                        {
                            response.Add("ERROR: Command " + args[3] + " tidak ditemukan");
                        }
                    }
                    else
                    {
                        response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                    }
                }
                else
                {
                    response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
                }
            }
        }
        else
        {
            InputUmum();
        }
    }

    private void QuestIntroNc()
    {
        // install tools
        if (args[0] == "apt-get")
        {
            if (args.Length == 1)
            {
                response.Add("ERROR: Command " + args[0] + " tidak memiliki parameter");
            }
            else if (args[1] == "install")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Tools belum dimasukkan");
                }
                else if (args[2] == "netcat")
                {

                    response.Add("Reading package lists… Done");
                    response.Add("Building dependency tree");
                    response.Add("Reading state information… Done");
                    response.Add("netcat is already the newest version.");
                    response.Add("0 upgraded, 0 newly installed, 0 to remove and 0 not upgraded.");
                    response.Add("");
                    netcatInstalled = true;
                    SaveInstalledTools();
                    Debug.Log("netcat " + netcatInstalled);
                }
                else
                {
                    response.Add("ERROR: Command " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "nc" && netcatInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format: nc -lnvp 4444");
                response.Add("");
            }
            else if (args[1] == "-lnvp")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Port belum dimasukkan");
                }
                else if (args[2] == "4444")
                {
                    TerminalManager.netcat = true;
                    SaveInstalledTools();
                    response.Add("listening on [any] 4444 ...");
                    response.Add("connect to [192.168.43.4] from (UNKNOWN)");
                }
                else
                {
                    response.Add("ERROR: Port " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "ls" && TerminalManager.netcat)
        {
            if (args.Length == 1)
            {
                response.Add("documents");
                response.Add("downloads");
                response.Add("music");
                response.Add("movies");
                response.Add("");
            }
            else if (args[1] == "documents")
            {
                response.Add("list.txt");
                response.Add("");
            }
            else if (args[1] == "downloads")
            {
                response.Add("pict.txt");
                response.Add("");
            }
            else if (args[1] == "music")
            {
                response.Add("bangbang.txt");
                response.Add("");
            }
            else if (args[1] == "movies")
            {
                response.Add("midnight.txt");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "cat" && TerminalManager.netcat)
        {
            if (args.Length == 1)
            {
                response.Add("Format: cat [folder]/[nama file]");
            }
            else if (args[1] == "documents/list.txt")
            {
                response.Add("Jl. Pahlawan No. 78");
                response.Add("");
            }
            else if (args[1] == "downloads/pict.txt")
            {
                response.Add("082 178 362 881");
                response.Add("");
            }
            else if (args[1] == "music/bangbang.txt")
            {
                response.Add("Music bangbang");
                response.Add("");
            }
            else if (args[1] == "movies/midnight.txt")
            {
                response.Add("Movie ini berjudul midnight");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }

        }
        else if (args[0] == "download" && TerminalManager.netcat)
        {
            // this
            if (args[1] == "documents/list.txt")
            {
                response.Add("Downloading file \"list.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 13;
                SaveInstalledTools();
            }
            else if (args[1] == "downloads/pict.txt")
            {
                response.Add("Downloading file \"pict.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 14;
                SaveInstalledTools();
            }
            else if (args[1] == "music/bangbang.txt")
            {
                response.Add("Downloading file \"bangbang.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 15;
                SaveInstalledTools();
            }
            else if (args[1] == "movies/midnight.txt")
            {
                response.Add("Downloading file \"midnight.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 16;
                SaveInstalledTools();
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "exit")
        {
            TerminalManager.netcat = false;
            SaveInstalledTools();
        }
        else
        {
            InputUmum();
        }
    }

    private void QuestNc1()
    {
        if (args[0] == "nc" && netcatInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format: nc -lnvp 4444");
                response.Add("");
            }
            else if (args[1] == "-lnvp")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Port belum dimasukkan");
                }
                else if (args[2] == "4444")
                {
                    TerminalManager.netcat = true;
                    SaveInstalledTools();
                    response.Add("listening on [any] 4444 ...");
                    response.Add("connect to [192.168.43.4] from (UNKNOWN)");
                }
                else
                {
                    response.Add("ERROR: Port " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "ls" && TerminalManager.netcat)
        {
            if (args.Length == 1)
            {
                response.Add("favorite");
                response.Add("deadline");
                response.Add("movies");
                response.Add("network");
                response.Add("");
            }
            else if (args[1] == "favorite")
            {
                response.Add("memo.txt");
                response.Add("");
            }
            else if (args[1] == "deadline")
            {
                response.Add("today.txt");
                response.Add("");
            }
            else if (args[1] == "movies")
            {
                response.Add("counter.txt");
                response.Add("");
            }
            else if (args[1] == "network")
            {
                response.Add("ipadd.txt");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "cat" && TerminalManager.netcat)
        {
            if (args.Length == 1)
            {
                response.Add("Format: cat [folder]/[nama file]");
            }
            else if (args[1] == "favorite/memo.txt")
            {
                response.Add("Ini catatan pribadiku");
                response.Add("");
            }
            else if (args[1] == "deadline/today.txt")
            {
                response.Add("2736 0183 7391 6391");
                response.Add("");
            }
            else if (args[1] == "movies/counter.txt")
            {
                response.Add("087 638 739 183");
                response.Add("");
            }
            else if (args[1] == "network/ipadd.txt")
            {
                response.Add("Ip saya 202.189.28.3");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }

        }
        else if (args[0] == "download" && TerminalManager.netcat)
        {
            // this
            if (args[1] == "favorite/memo.txt")
            {
                response.Add("Downloading file \"memo.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 17;
                SaveInstalledTools();
            }
            else if (args[1] == "deadline/today.txt")
            {
                response.Add("Downloading file \"today.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 18;
                SaveInstalledTools();
            }
            else if (args[1] == "movies/counter.txt")
            {
                response.Add("Downloading file \"counter.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 19;
                SaveInstalledTools();
            }
            else if (args[1] == "network/ipadd.txt")
            {
                response.Add("Downloading file \"ipadd.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 20;
                SaveInstalledTools();
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "exit")
        {
            TerminalManager.netcat = false;
            SaveInstalledTools();
        }
        else
        {
            InputUmum();
        }
    }

    private void QuestNc2()
    {
        if (args[0] == "nc" && netcatInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format: nc -lnvp 4444");
                response.Add("");
            }
            else if (args[1] == "-lnvp")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Port belum dimasukkan");
                }
                else if (args[2] == "4444")
                {
                    TerminalManager.netcat = true;
                    SaveInstalledTools();
                    response.Add("listening on [any] 4444 ...");
                    response.Add("connect to [192.168.43.4] from (UNKNOWN)");
                }
                else
                {
                    response.Add("ERROR: Port " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "ls" && TerminalManager.netcat)
        {
            if (args.Length == 1)
            {
                response.Add("documents");
                response.Add("file");
                response.Add("music");
                response.Add("videos");
                response.Add("");
            }
            else if (args[1] == "documents")
            {
                response.Add("file.txt");
                response.Add("");
            }
            else if (args[1] == "file")
            {
                response.Add("november.txt");
                response.Add("");
            }
            else if (args[1] == "music")
            {
                response.Add("File tidak ditemukan");
                response.Add("");
            }
            else if (args[1] == "videos")
            {
                response.Add("yesterday.txt");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "cat" && TerminalManager.netcat)
        {
            if (args.Length == 1)
            {
                response.Add("Format: cat [folder]/[nama file]");
            }
            else if (args[1] == "documents/file.txt")
            {
                response.Add("Ini berkas saya");
                response.Add("");
            }
            else if (args[1] == "file/november.txt")
            {
                response.Add("9183 9173 6821 7382");
                response.Add("");
            }
            else if (args[1] == "videos/yesterday.txt")
            {
                response.Add("Mini vlog 12/12/2023");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }

        }
        else if (args[0] == "download" && TerminalManager.netcat)
        {
            if (args[1] == "documents/file.txt")
            {
                response.Add("Downloading file \"file.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 21;
                SaveInstalledTools();
            }
            // this
            else if (args[1] == "file/november.txt")
            {
                response.Add("Downloading file \"november.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 22;
                SaveInstalledTools();
            }
            else if (args[1] == "videos/yesterday.txt")
            {
                response.Add("Downloading file \"yesterday.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 23;
                SaveInstalledTools();
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "exit")
        {
            TerminalManager.netcat = false;
            SaveInstalledTools();
        }
        else
        {
            InputUmum();
        }
    }

    private void QuestNc3()
    {
        if (args[0] == "nc" && netcatInstalled)
        {
            if (args.Length == 1)
            {
                response.Add("Format: nc -lnvp 4444");
                response.Add("");
            }
            else if (args[1] == "-lnvp")
            {
                if (args.Length == 2)
                {
                    response.Add("ERROR: Port belum dimasukkan");
                }
                else if (args[2] == "4444")
                {
                    TerminalManager.netcat = true;
                    SaveInstalledTools();
                    response.Add("listening on [any] 4444 ...");
                    response.Add("connect to [192.168.43.4] from (UNKNOWN)");
                }
                else
                {
                    response.Add("ERROR: Port " + args[2] + " tidak ditemukan");
                }
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "ls" && TerminalManager.netcat)
        {
            if (args.Length == 1)
            {
                response.Add("applications");
                response.Add("favorite");
                response.Add("music");
                response.Add("zip");
                response.Add("");
            }
            else if (args[1] == "applications")
            {
                response.Add("zuma.txt");
                response.Add("");
            }
            else if (args[1] == "favorite")
            {
                response.Add("File tidak ditemukan");
                response.Add("");
            }
            else if (args[1] == "music")
            {
                response.Add("logo.txt");
                response.Add("");
            }
            else if (args[1] == "zip")
            {
                response.Add("private.txt");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "cat" && TerminalManager.netcat)
        {
            if (args.Length == 1)
            {
                response.Add("Format: cat [folder]/[nama file]");
            }
            else if (args[1] == "applications/zuma.txt")
            {
                response.Add("Game zuma ada di sini");
                response.Add("");
            }
            else if (args[1] == "music/logo.txt")
            {
                response.Add("Jl. Bangka No. 65");
                response.Add("");
            }
            else if (args[1] == "zip/private.txt")
            {
                response.Add("Isinya sangat rahasia, jangan dibuka");
                response.Add("");
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }

        }
        else if (args[0] == "download" && TerminalManager.netcat)
        {
            if (args[1] == "applications/zuma.txt")
            {
                response.Add("Downloading file \"zuma.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 24;
                SaveInstalledTools();
            }
            // this
            else if (args[1] == "music/logo.txt")
            {
                response.Add("Downloading file \"logo.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 25;
                SaveInstalledTools();
            }
            else if (args[1] == "zip/private.txt")
            {
                response.Add("Downloading file \"private.txt\"");
                response.Add("Download complete");
                response.Add("");
                fileDownload = 26;
                SaveInstalledTools();
            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }
        }
        else if (args[0] == "exit")
        {
            TerminalManager.netcat = false;
            SaveInstalledTools();
        }
        else
        {
            InputUmum();
        }
    }

    private void InputUmum()
    {
        ipAdd = IsIPAddressValid(args.Length > 1 ? args[1] : "");

        // format belum selesai
        if (args[0] == "airodump-ng" && aircrackInstalled)
        {
            response.Add("Format:");
            response.Add("Untuk memantau jaringan wifi:");
            response.Add("airodump-ng [interface]");
            response.Add("Untuk menangkap file handshake:");
            response.Add("airodump-ng -w [nama file handshake] -c [channel] -bssid [bssid]");
            response.Add("");
        }
        else if (args[0] == "aireplay-ng" && aircrackInstalled)
        {
            response.Add("Format: aireplay-ng --deauth -0 -a [bssid] [interface]");
            response.Add("");
        }
        else if (args[0] == "aircrack-ng" && aircrackInstalled)
        {
            response.Add("Format: aircrack-ng [nama file handshake] -w [file wordlists]");
            response.Add("");
        }
        else if (args[0] == "airmon-ng" && aircrackInstalled)
        {
            response.Add("Format: airmon-ng [start/stop] [interface]");
            response.Add("");
        }
        else if (args[0] == "nmap" && nmapInstalled)
        {
            response.Add("Format:");
            response.Add("nmap [ip] \t\t\t\t Melakukan scanning sederhana");
            response.Add("nmap -sV [ip] \t\t\t Melakukan scanning version yang digunakan");
            response.Add("nmap -p [port] [ip] \t\t\t Melakukan scanning pada port tertentu");
            response.Add("nmap [ip network]/[prefix] \t\t Melakukan scanning pada satu jaringan");
            response.Add("nmap -O [ip] \t\t\t Mengecek OS yang digunakan target");
            response.Add("nmap -sn [ip network]/[prefix] \t Melakukan scanning network");
            response.Add("");
        }
        else if (args[0] == "snort" && snortInstalled)
        {
            response.Add("Format: snort [parameter]");
            response.Add("");
        }
        else if (args[0] == "nc" && netcatInstalled)
        {
            response.Add("Format: nc -lnvp 4444");
            response.Add("");
        }
        else if (args[0] == "python3")
        {
            response.Add("Format: python3 [nama folder]/[nama file exploit] [ip target]");
            response.Add("");
        }

        // ping
        else if (args[0] == "ping" && ipAdd)
        {
            response.Add("PING " + args[1] + " 56(84) bytes of data.");
            response.Add("Request time out.");
            response.Add("Request time out.");
            response.Add("Request time out.");
            response.Add("Request time out.");
            response.Add("");
            response.Add("--- " + args[1] + " ping statistics ---");
            response.Add("4 packets transmitted, 0 received, 100 % packet loss, time 5113ms");
            response.Add("rtt min / avg / max / mdev = 0.043 / 0.060 / 0.068 / 0.008 ms");
            response.Add("");
        }
        else if (args[0] == "ping")
        {
            response.Add("Format: ping [ip]");
            response.Add("");
        }

        // help
        else if (args[0] == "help")
        {
            if (TerminalManager.meterpreter || TerminalManager.netcat)
            {
                response.Add("Command:");
                response.Add("ls [folder] \t\t Untuk melihat isi folder");
                response.Add("cat [folder]/[nama file] \t Untuk membaca isi file");
                response.Add("download [folder]/nama file \t Untuk mengambil file dari target");
                response.Add("exit \t\t\t Untuk kelauar dari target");
                response.Add("");
            }
            else
            {
                response.Add("Command:");
                response.Add("ping [ip] \t\t\t Untuk mengecek apakah target online");
                response.Add("ls [folder] \t\t Untuk melihat isi folder");
                response.Add("airmon-ng \t\t\t Untuk mengaktifkan mode monitor");
                response.Add("airodump-ng \t\t Untuk menampilkan jaringan wifi disekitar");
                response.Add("aireplay-ng \t\t Untuk menangkap file handshake");
                response.Add("aircrack-ng \t\t Untuk mengecrack file handshake");
                response.Add("snort \t\t\t Untuk melihat lalu lintas jaringan");
                response.Add("nmap \t\t\t Untuk melakukan scanning pada sebuah jaringan");
                response.Add("nc \t\t\t Untuk melakukan reverse shell (netcat)");
                response.Add("");
            }

        }

        // ls
        else if (args[0] == "ls")
        {
            if (args.Length == 1)
            {
                response.Add("download");
                response.Add("desktop");
                response.Add("pictures");
                response.Add("videos");
                if (handshakeFile)
                {
                    response.Add(handshake + "-01.cap");
                }
                response.Add("wordlists.txt");

                // intro exploit
                if (fileDownload == 1) { response.Add("report.txt"); }
                else if (fileDownload == 2) { response.Add("home.txt"); }
                else if (fileDownload == 3) { response.Add("movie123.txt"); }
                else if (fileDownload == 4) { response.Add("hellopanda.txt"); }

                // exploit 1
                else if (fileDownload == 5) { response.Add("my.txt"); }
                else if (fileDownload == 6) { response.Add("task1.txt"); }
                else if (fileDownload == 7) { response.Add("task2.txt"); }
                else if (fileDownload == 8) { response.Add("take1.txt"); }

                // exploit 2
                else if (fileDownload == 9) { response.Add("record.txt"); }
                else if (fileDownload == 10) { response.Add("music1.txt"); }
                else if (fileDownload == 11) { response.Add("account.txt"); }
                else if (fileDownload == 12) { response.Add("bank.txt"); }

                // intro netcat
                else if (fileDownload == 13) { response.Add("list.txt"); }
                else if (fileDownload == 14) { response.Add("pict.txt"); }
                else if (fileDownload == 15) { response.Add("bangbang.txt"); }
                else if (fileDownload == 16) { response.Add("midnight.txt"); }

                // netcat 1
                else if (fileDownload == 17) { response.Add("memo.txt"); }
                else if (fileDownload == 18) { response.Add("today.txt"); }
                else if (fileDownload == 19) { response.Add("counter.txt"); }
                else if (fileDownload == 20) { response.Add("ipadd.txt"); }

                // netcat 2
                else if (fileDownload == 21) { response.Add("file.txt"); }
                else if (fileDownload == 22) { response.Add("november.txt"); }
                else if (fileDownload == 23) { response.Add("yesterday.txt"); }

                // netcat 3
                else if (fileDownload == 24) { response.Add("zuma.txt"); }
                else if (fileDownload == 25) { response.Add("logo.txt"); }
                else if (fileDownload == 26) { response.Add("private.txt"); }
                else
                {

                }
                response.Add("");

                fileDownload = 0;
                SaveInstalledTools();
            }
            else if (args[1] == "download")
            {
                if (FileManager.downloadedFiles.Count > 0)
                {
                    for (int i = 0; i < FileManager.downloadedFiles.Count; i++)
                    {
                        response.Add(FileManager.downloadedFiles[i]);
                    }
                    response.Add("");
                }
                else
                {
                    response.Add("File tidak ditemukan");
                    response.Add("");
                }
            }
            else if (args[1] == "desktop")
            {
                response.Add("File tidak ditemukan");
                response.Add("");
            }
            else if (args[1] == "pictures")
            {
                response.Add("File tidak ditemukan");
                response.Add("");
            }
            else if (args[1] == "videos")
            {
                response.Add("File tidak ditemukan");
                response.Add("");
            }
            //snort
            else if (args[1] == "/var/log/snort/")
            {
                if (packetSnort1)
                {
                    response.Add("snort.log.123");
                    response.Add("");
                }
                else if (packetSnort2)
                {
                    response.Add("snort.log.716");
                    response.Add("");
                }
                else
                {
                    response.Add("File tidak ditemukan");
                    response.Add("");
                }

            }
            else
            {
                response.Add("ERROR: Command " + args[1] + " tidak ditemukan");
            }

        }
        else
        {
            response.Add("ERROR: Command " + args[0] + " tidak ditemukan");
        }
    }
}