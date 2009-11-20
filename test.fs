INCLUDE kernel.fs
INCLUDE bios.fs

: MESSAGE
    BIN 10101010 DEC 520 WRTVRM
    
    S" AAAAAAAAAAAAA" TYPE CR
    S" SamForth 2009" TYPE CR
    S" by Victor Marzo" TYPE CR
    S" <samsaga2@gmail.com>" TYPE CR CR ;

: MAIN
    32 LINL32 C!
    INIT32 0 0 15 CHGCLR
    MESSAGE ABORT ;
