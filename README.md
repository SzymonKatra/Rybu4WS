# Rybu4WS

This repository contains source of **Rybu4WS language parser** and **Rybu4WS Debugger** application.

Rybu4WS was designed as a part of my Master's thesis titled [**Specification and verification of Web Service composition in DedAn environment**](https://repo.pw.edu.pl/info/master/WUTfe509ac3967748c8b159e81620f9a93c/).

Additionaly, it it a subject of [**An Experimentation Framework for Specification and Verification of Web Services**](https://ieeexplore.ieee.org/document/9908728) article published in [*Proceedings of the 17th Conference on Computer Science and Intelligence Systems*](https://ieeexplore.ieee.org/xpl/conhome/9908518/proceeding).  
---> PDF available [**here**](https://annals-csis.org/Volume_30/pliks/fedcsis.pdf), page 913.

## What is Rybu4WS?

The Rybu4WS language was developed for modeling Web Service compositions, overcoming the limitations imposed by [Rybu](https://github.com/zyla/rybu). Rybu4WS is based on the original Rybu language but is not backward compatible and features more advanced functionality which is necessary for Web Service composition:
- server-server communication that allows agents to travel between different
servers and execute complex scenarios,
- state variables in grouped processes, which enables the communication between
different processes without sending actual IMDS messages,
- termination at any point of execution,
- complex code sequences in reactive server actions instead of trivial state mutation
and return value.

Rybu4WS language is used only for modeling distributed systems and cannot be directly verified against deadlocks or terminations.
For the purpose of verification in the [DedAn](http://staff.ii.pw.edu.pl/dedan/) environment, Rybu4WS code must be converted into IMDS equivalent using a set of unambiguous translation rules.

## Rybu4WS Debugger

Rybu4WS Debugger is a desktop app that allows to load Rybu4WS code and convert it into corresponding IMDS representation for the purpose of verification in the DedAn
environment.
After verification, DedAn may present a counterexample/witness, which can be visualized in a user-friendly way for the manual analysis.

## Example Rybu4WS source code
```
type BOOL = { t, f };

server Payment {
    var s: { none, pending, paid };
    { Init | s == :none } -> { return :ok; }
    { Confirm | s == :pending } -> { s = :paid; return :ok; }
    { IsPaid | s == :paid } -> { return :t; }
}

server Bank(p: Payment) {
    var bal: 0..5;
    var s: BOOL;
    { Transfer | bal > 0 && s == :f } -> {
        s = :t;
        return :confReq;
    }
    { Transfer | bal == 0 || s == :t } -> {
        return :fail;
    }
    { Confirm | s == :t && bal > 0 } -> {
        bal -= 1;
        s = :f;
        p.Confirm();
        return :ok;
    }
}

server Warehouse() {
    var x: BOOL;
    { Reserve | x == :f } -> { x = :t; return :ok; }
    { Reserve | x == :f } -> { return :outOfStock; }
    { Dispatch | x == :t } -> { x = :f; return :ok; }
}

server BookShop(w: Warehouse, p: Payment) {
    { Begin } -> {
        match w.Reserve() {
            :outOfStock -> { return :fail; }
            :ok -> { p.Init(); return :payReq; }
        }
    }
    { End } -> {
        match p.IsPaid() {
            :t -> { w.Dispatch(); return :ok; }
        }
    }
}

var p = Payment() { s = :none };
var b = Bank(p) { bal = 3, s = :f };
var w = Warehouse() { x = :f };
var bs = BookShop(w, p);

group BookPurchaseScenario {
    var action: { idle, none, pay } = :idle;
    
    process UserWebInterface {
        match bs.Begin() {
            :fail -> { action = :none; terminate; }
            :payReq -> {
                match b.Transfer() {
                    :confReq -> {
                        action = :pay; bs.End(); terminate;
                    }
                    :fail -> { terminate; }
                }
            }
        }
    }

    process UserMobileApp {
        wait(action != :idle);
        if (action == :pay) { b.Confirm(); }
        terminate;
    }
}
```
