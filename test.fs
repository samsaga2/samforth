INCLUDE kernel.fs
INCLUDE bios.fs

binary
create test-char 
00000000 c,
00000000 c,
00000000 c,
00001111 c,
11110000 c,
00000000 c,
00000000 c,
00000000 c,
00000000 c,
decimal

: char-addr ( c -- v-addr )
    3 lshift ;

: bold-byte ( i -- j )
    dup 1 rshift or ;

: bold-font ( -- )
    127 char-addr 7 +
    32 char-addr
    do
        i vram@
        bold-byte i vram!
    loop ;

: redefine-char ( char addr -- )
    >r 8 swap 3 lshift r> ram-to-vram ;

: show-message
    ." ************************** " cr
    ." SamForth " 2009 decimal . cr
    ." by Victor Marzo" cr
    ." <samsaga2@gmail.com>" cr
    ." ************************** " cr cr ;

: init-screen
    32 linl32 c!
    init-mode32 0 0 15 change-color
    bold-font
    [char] * test-char redefine-char ;

: main
    init-screen show-message abort ;
