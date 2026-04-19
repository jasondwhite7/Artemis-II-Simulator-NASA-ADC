using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class AntennaPicker : MonoBehaviour
{
    public TextMeshProUGUI Switches;

    GameObject Display;
    List<List<int>> AntennasNom;
    List<List<float>> LinkBudgetsNom;
    List<List<int>> AntennasOff;
    List<List<float>> LinkBudgetsOff;
    List<float> Times = new();
    int balanceSwitchesNom = 0;
    int leastSwitchesNom = 0;
    int linkSwitchesNom = 0;
    int balanceAntennaNom = 0;
    int switchesAntennaNom = 0;
    int linkAntennaNom = 0;
    int balanceSwitchesOff = 0;
    int leastSwitchesOff = 0;
    int linkSwitchesOff = 0;
    int balanceAntennaOff = 0;
    int switchesAntennaOff = 0;
    int linkAntennaOff = 0;
    bool WPSAFixB = true;
    bool WPSAFixS = true;
    bool WPSAFixL = true;
    bool offNom = false;
    string algorithm = "balance";
    string[] antennaNames = {
        "WPSADisplay",
        "DS54Display",
        "DS24Display",
        "DS34Display"
    };
    // Start is called before the first frame update
    void Start()
    {
        Display = GameObject.Find("AntennaDisplay");
    }

    public int PickAntennaBalanced(int index, out int switches, bool offCall, List<List<float>> linkBudgets, List<List<int>> antennas) {
        // set connected and switches variables based on offCall
        int connect = offCall ? balanceAntennaOff : balanceAntennaNom;
        switches = offCall ? balanceSwitchesOff : balanceSwitchesNom;
        // update availability
        int balanceAvailability = antennas[connect][index];
        if (antennas[0][index]
            + antennas[1][index]
            + antennas[2][index]
            + antennas[3][index] == 0) {
                return 0;
        }
        // set a "memory availability" so as to not switch antennas
        // if availability variable does not change
        int oldAvailability = balanceAvailability;
        if (WPSAFixB) {
            oldAvailability = 8;
            WPSAFixB = false;
        }
        for (int i = 0; i < 4; i++) {
            // check if another antenna has both greater availability and
            // link budget than the currently connected antenna
            // if (linkBudgets[i][index] > 4 * linkBudgets[connect][index]) {
                // balanceAvailability = antennas[i][index];
                // connect = i;
            // } else {
                if ((antennas[i][index] > balanceAvailability) && (linkBudgets[i][index] > linkBudgets[connect][index])) {
                    balanceAvailability = antennas[i][index];
                    connect = i;
                } else {
                    continue;
                // }
            }
        }
        // only switch antennas if the availability variable changed
        if (balanceAvailability != oldAvailability) {
            switches++;
        }
        return connect;
    }

    public int PickAntennaSwitches(int index, out int switches, bool offCall, List<List<float>> linkBudgets, List<List<int>> antennas) {
        // set connected and switches variables based on offCall
        int connect = offCall ? switchesAntennaOff : switchesAntennaNom;
        switches = offCall ? leastSwitchesOff : leastSwitchesNom;
        // update availability
        int mostAvailability = antennas[connect][index];
        if (antennas[0][index]
            + antennas[1][index]
            + antennas[2][index]
            + antennas[3][index] == 0) {
                return 0;
        }
        if (WPSAFixS) {
            switches++;
            WPSAFixS = false;
        }
        if (antennas[connect][index] != 0) {
            // if (algorithm == "switches") { currentAntenna = switchesAntenna; }
            return connect;
        } else {
            // pick a new antenna if currently connected one is unavailable,
            // based on which available one will be available longest
            for (int i = 0; i < 4; i++) {
                // this loop iterates through the antennas and sorts the
                // "availability length" to set to availability
                if (mostAvailability > antennas[i][index]) {
                    continue;
                } else {
                    mostAvailability = antennas[i][index];
                    connect = i;
                }
            }
            // if (algorithm == "switches") { currentAntenna = switchesAntenna; }
            switches++;
        }
        return connect;
    }

    public int PickAntennaLink(int index, out int switches, bool offCall, List<List<float>> linkBudgets, List<List<int>> antennas) {
        // set connected and switches variables based on offCall
        int connect = offCall? linkAntennaOff : linkAntennaNom;
        switches = offCall ? linkSwitchesOff : linkSwitchesNom;
        // update availability
        int linkAvailability = antennas[connect][index];
        if (antennas[0][index]
            + antennas[1][index]
            + antennas[2][index]
            + antennas[3][index] == 0) {
                return 0;
        }
        // same oldAvailability thing as PickAntennaBalanced
        int oldAvailability = linkAvailability;
        if (WPSAFixL) {
            oldAvailability = 8;
            WPSAFixL = false;
        }
        for (int i = 0; i < 4; i++) {
            // just compares link budgets when deciding whether to switch or not
            if (linkBudgets[i][index] > linkBudgets[connect][index]) {
                linkAvailability = antennas[i][index];
                connect = i;
            } else {
                continue;
            }
        }
        // if (algorithm == "link") { currentAntenna = linkAntenna; }
        if (linkAvailability != oldAvailability) {
            switches++;
        }
        return connect;
    }

    // function to receive data from timer
    public void CreateLists(List<TimeSplice> list) {
        // put argument for SendMessage as 'new object[] {nominalList, offNomList}'
        List<float> WPSANom = new();
        List<float> DS54Nom = new();
        List<float> DS24Nom = new();
        List<float> DS34Nom = new();
        

        foreach (TimeSplice item in list) {
            WPSANom.Add(item.LinkBudgetWPSA);
            DS54Nom.Add(item.LinkBudgetDS54);
            DS24Nom.Add(item.LinkBudgetDS24);
            DS34Nom.Add(item.LinkBudgetDS34);
            Times.Add(item.Time);
        }

        LinkBudgetsNom = new() {
            WPSANom,
            DS54Nom,
            DS24Nom,
            DS34Nom
        };
        
        AntennasNom = new() {
            CalculateConnections(WPSANom),
            CalculateConnections(DS54Nom),
            CalculateConnections(DS24Nom),
            CalculateConnections(DS34Nom)
        };
    }
    public void CreateOffNomLists(List<BonusTimeSplice> list) {
        List<float> WPSAOff = new();
        List<float> DS54Off = new();
        List<float> DS24Off = new();
        List<float> DS34Off = new();

        foreach (BonusTimeSplice item in list) {
            WPSAOff.Add(item.LinkBudgetWPSA);
            DS54Off.Add(item.LinkBudgetDS54);
            DS24Off.Add(item.LinkBudgetDS24);
            DS34Off.Add(item.LinkBudgetDS34);
        }

        LinkBudgetsOff = new() {
            WPSAOff,
            DS54Off,
            DS24Off,
            DS34Off
        };

        AntennasOff = new() {
            CalculateConnections(WPSAOff),
            CalculateConnections(DS54Off),
            CalculateConnections(DS24Off),
            CalculateConnections(DS34Off)
        };
    }
    public List<int> CalculateConnections(List<float> linkBudgets) {
        List<int> connections = new();
        int availability = 0;
        for (int i = 0; i < linkBudgets.Count; i++) {
            // if antenna unavailable, set to 0
            if (linkBudgets[i] == 0) {
                connections.Add(0);
                continue;
            }
            if (availability == 0) {
                // for loop to check for how many time splices an antenna
                // is available for, and set that number to availability
                /*
                    j increments until linkBudgets[i + j] is 0. i + j is so that
                    this loop looks at where i is currently in the list,
                    as opposed to the beginning of the list, which would not
                    be good. Going until linkBudgets[i + j] is 0 is to see how
                    long until the antenna becomes unavailable, and that many
                    splices is what j is, which availability is set to.
                */
                for (int j = 0; linkBudgets[i + j] != 0; j++) {
                    availability = j;
                }
            } else {
                // if availability is 0, decrement availability
                /*
                    The reasoning behind this is because availability could be 50,
                    meaning the antenna would be available for 50 more splices.
                    this is now on the next splice, where the antenna is
                    available for 50 - 1 more splices, so availability is
                    decremented. This goes until it hits 0, where the antenna
                    is no longer available. Ask Nat for explanation if needed!
                */
                availability--;
            }
            // put availability in the list
            connections.Add(availability);
            // fix availability in list because it calculates
            // 1 less than actual value
            connections[i]++;
        }
        return connections;
    }
    // this function handles handles which algorithm to display
    // it also handles updating the availability times, since that is universal
    public void PickAntenna(int index) {
        List<float> times = new();
        // run the algorithm functions
        balanceAntennaNom = PickAntennaBalanced(index, out balanceSwitchesNom, false, LinkBudgetsNom, AntennasNom);
        switchesAntennaNom = PickAntennaSwitches(index, out leastSwitchesNom, false, LinkBudgetsNom, AntennasNom);
        linkAntennaNom = PickAntennaLink(index, out linkSwitchesNom, false, LinkBudgetsNom, AntennasNom);
        balanceAntennaOff = PickAntennaBalanced(index, out balanceSwitchesOff, true, LinkBudgetsOff, AntennasOff);
        switchesAntennaOff = PickAntennaSwitches(index, out leastSwitchesOff, true, LinkBudgetsOff, AntennasOff);
        linkAntennaOff = PickAntennaLink(index, out linkSwitchesOff, true, LinkBudgetsOff, AntennasOff);
        // nominal or off nominal
        if (!offNom) {
            if (algorithm == "balance") {
                Display.SendMessage("AntennaSwitch", balanceAntennaNom);
                Switches.text = $"TOTAL SWITCHES: {balanceSwitchesNom}";
            } else if (algorithm == "switches") {
                Display.SendMessage("AntennaSwitch", switchesAntennaNom);
                Switches.text = $"TOTAL SWITCHES: {leastSwitchesNom}";
            } else if (algorithm == "link") {
                Display.SendMessage("AntennaSwitch", linkAntennaNom);
                Switches.text = $"TOTAL SWITCHES: {linkSwitchesNom}";
            } else {
                Display.SendMessage("AntennaSwitch", balanceAntennaNom);
                Switches.text = $"TOTAL SWITCHES: {balanceSwitchesNom}";
            }
	        Display.SendMessage("WPSADisplay", LinkBudgetsNom[0][index]);
	        Display.SendMessage("DS54Display", LinkBudgetsNom[1][index]);
	        Display.SendMessage("DS24Display", LinkBudgetsNom[2][index]);
	        Display.SendMessage("DS34Display", LinkBudgetsNom[3][index]);
            times.Add(Times[AntennasNom[0][index] + index] - Times[index]);
            times.Add(Times[AntennasNom[1][index] + index] - Times[index]);
            times.Add(Times[AntennasNom[2][index] + index] - Times[index]);
            times.Add(Times[AntennasNom[3][index] + index] - Times[index]);
            for (int i = 0; i < 4; i++) {
                Display.SendMessage(antennaNames[i], LinkBudgetsNom[i][index]);
            }
        } else {
            if (algorithm == "balance") {
                Display.SendMessage("AntennaSwitch", balanceAntennaOff);
                Switches.text = $"TOTAL SWITCHES: {balanceSwitchesOff}";
            } else if (algorithm == "switches") {
                Display.SendMessage("AntennaSwitch", switchesAntennaOff);
                Switches.text = $"TOTAL SWITCHES: {leastSwitchesOff}";
            } else if (algorithm == "link") {
                Display.SendMessage("AntennaSwitch", linkAntennaOff);
                Switches.text = $"TOTAL SWITCHES: {linkSwitchesOff}";
            } else {
                Display.SendMessage("AntennaSwitch", balanceAntennaOff);
                Switches.text = $"TOTAL SWITCHES: {balanceSwitchesOff}";
            }
	        Display.SendMessage("WPSADisplay", LinkBudgetsOff[0][index]);
	        Display.SendMessage("DS54Display", LinkBudgetsOff[1][index]);
	        Display.SendMessage("DS24Display", LinkBudgetsOff[2][index]);
	        Display.SendMessage("DS34Display", LinkBudgetsOff[3][index]);
            times.Add(Times[AntennasOff[0][index] + index] - Times[index]);
            times.Add(Times[AntennasOff[1][index] + index] - Times[index]);
            times.Add(Times[AntennasOff[2][index] + index] - Times[index]);
            times.Add(Times[AntennasOff[3][index] + index] - Times[index]);
            for (int i = 0; i < 4; i++) {
                Display.SendMessage(antennaNames[i], LinkBudgetsOff[i][index]);
            }
        }
        // send availability times to AntennaDisplay
        Display.SendMessage("AvailabilityTimes", times);
    }
    // the buttons call this function to let the script know which algorithm to display
    public void SwitchAlgorithm(object[] args) {
        algorithm = (string)args[0];
        PickAntenna((int)args[1]);
    }
    public void SwitchNom(object[] args) {
        offNom = (bool)args[0];
        PickAntenna((int)args[1]);
    }
    public void ResetSwitches() {
        balanceSwitchesNom = 0;
        leastSwitchesNom = 0;
        linkSwitchesNom = 0;
        balanceAntennaNom = 0;
        switchesAntennaNom = 0;
        linkAntennaNom = 0;
        balanceSwitchesOff = 0;
        leastSwitchesOff = 0;
        linkSwitchesOff = 0;
        balanceAntennaOff = 0;
        switchesAntennaOff = 0;
        linkAntennaOff = 0;
        WPSAFixB = true;
        WPSAFixS = true;
        WPSAFixL = true;
    }
}