HEX

-- System vars
F3AE CONST LINL40
F3AF CONST LINL32
F3E9 CONST FORCLR
F3EA CONST BAKCLR
F3EB CONST BDRCLR

: DISABLE-SCREEN ( -- )
    0 0 0 0 0041 SYSCALL 4DROP ;

: ENABLE-SCREEN ( -- )
    0 0 0 0 0044 SYSCALL 4DROP ;

: VDP! ( data register -- )
    8 LSHIFT + >R
    0 R> 0 0 0047 SYSCALL 4DROP ;

: VRAM@ ( address -- data )
    >R 0 0 0 R> 004A SYSCALL 3DROP ;

: VRAM! ( data address -- )
    >R 0 0 R> 004D SYSCALL 4DROP ;

: CURSOR-POSITION ( column row -- )
    8 LSHIFT + >R
    0 0 0 R> 00C6 SYSCALL 4DROP ;

: CHANGE-COLOR ( bordercolor backgroundcolor foregroundcolor -- )
    FORCLR C! BAKCLR C! BDRCLR C!
    0 0 0 0 0062 SYSCALL 4DROP ;

: INIT-TEXT ( -- )
    0 0 0 0 006C SYSCALL 4DROP ;

: INIT-MODE32 ( -- )
    0 0 0 0 006F SYSCALL 4DROP ;

: INIT-GRAPH ( -- )
    0 0 0 0 0072 SYSCALL 4DROP ;

DEC
