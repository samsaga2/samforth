INCLUDE kernel.fs
INCLUDE bios.fs

BIN
CREATE test-char 
00000000 C,
00111000 C,
01000100 C,
01000100 C,
01111100 C,
01100110 C,
01100110 C,
00000000 C,
00000000 C,
DEC

: BOLD ( -- )
    127 3 LSHIFT 7 +
    32 3 LSHIFT
    DO
        I VRAM@
        DUP 1 RSHIFT OR I VRAM!
    LOOP ;

: REDEFINE-A ( -- )
    8 [CHAR] A 3 LSHIFT test-char RAM-TO-VRAM ;
    
: MESSAGE
    S" AAAAAAAAAAAAA" TYPE CR
    S" SamForth 2009" TYPE CR
    S" by Victor Marzo" TYPE CR
    S" <samsaga2@gmail.com>" TYPE CR CR ;

: MAIN
    32 LINL32 C!
    INIT-MODE32 0 0 15 CHANGE-COLOR
    BOLD
    REDEFINE-A MESSAGE ABORT ;
