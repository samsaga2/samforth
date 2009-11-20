HEX

-- System vars
F3AE CONST LINL40
F3AF CONST LINL32
F3E9 CONST FORCLR
F3EA CONST BAKCLR
F3EB CONST BDRCLR

-- Disable screen display
: DISSCR ( -- )
    0 0 0 0 0041 SYSCALL 4DROP ;

-- Enable screen display
: ENASCR ( -- )
    0 0 0 0 0044 SYSCALL 4DROP ;

-- Writes to VDP register
: WRTVDP ( data register -- )
    8 LSHIFT + >R
    0 R> 0 0 0047 SYSCALL 4DROP ;

-- Read the VRAM address
: RDVRM ( address -- data )
    >R 0 0 0 R> 004A SYSCALL 3DROP ;

-- Write to the VRAM address
: WRTVRM ( data address -- )
    >R 0 0 R> 004D SYSCALL 4DROP ;

-- Locate cursor
: POSIT ( column row -- )
    8 LSHIFT + >R
    0 0 0 R> 00C6 SYSCALL 4DROP ;

-- Change color
: CHGCLR ( bordercolor backgroundcolor foregroundcolor -- )
    FORCLR C! BAKCLR C! BDRCLR C!
    0 0 0 0 0062 SYSCALL 4DROP ;

-- Switches to SCREEN 0
: INITXT ( -- )
    0 0 0 0 006C SYSCALL 4DROP ;

-- Switches to SCREEN 1
: INIT32 ( -- )
    0 0 0 0 006F SYSCALL 4DROP ;

-- Switches to SCREEN 2
: INIGRP ( -- )
    0 0 0 0 0072 SYSCALL 4DROP ;

DEC
