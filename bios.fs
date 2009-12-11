hex

\ colors
0 const COLOR-TRANSPARENT         
1 const COLOR-BLACK               
2 const COLOR-MEDIUM-GREEN        
3 const COLOR-LIGHT-GREEN         
4 const COLOR-DARK-BLUE           
5 const COLOR-LIGHT-BLUE          
6 const COLOR-DARK-RED            
7 const COLOR-CYAN                
8 const COLOR-MEDIUM-RED
9 const COLOR-LIGHT-RED
a const COLOR-DARK-YELLOW
b const COLOR-LIGHT-YELLOW
c const COLOR-DARK-GREEN
d const COLOR-MAGENTA
e const COLOR-GRAY
f const COLOR-WHITE

: color ( bg fg -- color )
    8 lshift or ;

\ system vars
F380 const SYS-RDPRIM	\ Routine that reads from a primary slot
F385 const SYS-WRPRIM	\ Routine that writes to a primary slot
F38C const SYS-CLPRIM	\ Routine that calls a routine in a primary slot
F39A const SYS-USRTAB	\ Address to call with Basic USR0
F3AE const SYS-LINL40	\ Width for SCREEN 0 (default 37)
F3AF const SYS-LINL32	\ Width for SCREEN 1 (default 29)
F3B0 const SYS-LINLEN	\ Width for the current text mode
F3B1 const SYS-CRTCNT	\ Number of lines on screen
F3B2 const SYS-CLMLST	\ Column space. It’s uncertain what this address actually stores
F3B3 const SYS-TXTNAM	\ BASE(0) - SCREEN 0 name table
F3B5 const SYS-TXTCOL	\ BASE(1) - SCREEN 0 color table
F3B7 const SYS-TXTCGP	\ BASE(2) - SCREEN 0 character pattern table
F3B9 const SYS-TXTATR	\ BASE(3) - SCREEN 0 Sprite Attribute Table
F3BB const SYS-TXTPAT	\ BASE(4) - SCREEN 0 Sprite Pattern Table
F3B3 const SYS-T32NAM	\ BASE(5) - SCREEN 1 name table
F3B5 const SYS-T32COL	\ BASE(6) - SCREEN 1 color table
F3B7 const SYS-T32CGP	\ BASE(7) - SCREEN 1 character pattern table
F3B9 const SYS-T32ATR	\ BASE(8) - SCREEN 1 sprite attribute table
F3BB const SYS-T32PAT	\ BASE(9) - SCREEN 1 sprite pattern table
F3B3 const SYS-GRPNAM	\ BASE(10) - SCREEN 2 name table
F3B5 const SYS-GRPCOL	\ BASE(11) - SCREEN 2 color table
F3B7 const SYS-GRPCGP	\ BASE(12) - SCREEN 2 character pattern table
F3B9 const SYS-GRPATR	\ BASE(13) - SCREEN 2 sprite attribute table
F3BB const SYS-GRPPAT	\ BASE(14) - SCREEN 2 sprite pattern table
F3B3 const SYS-MLTNAM	\ BASE(15) - SCREEN 3 name table
F3B5 const SYS-MLTCOL	\ BASE(16) - SCREEN 3 color table
F3B7 const SYS-MLTCGP	\ BASE(17) - SCREEN 3 character pattern table
F3B9 const SYS-MLTATR	\ BASE(18) - SCREEN 3 sprite attribute table
F3BB const SYS-MLTPAT	\ BASE(19) - SCREEN 3 sprite pattern table
F3DB const SYS-CLIKSW	\ =0 when key press click disabled =1 when key press click enabled
F3DC const SYS-CSRY	\ Current row-position of the cursor
F3DD const SYS-CSRX	\ Current column-position of the cursor
F3DE const SYS-CNSDFG	\ =0 when function keys are not displayed =1 when function keys are displayed
F3DF const SYS-RG0SAV	\ Content of VDP(0) register (R#0)
F3E0 const SYS-RG1SAV	\ Content of VDP(1) register (R#1)
F3E1 const SYS-RG2SAV	\ Content of VDP(2) register (R#2)
F3E2 const SYS-RG3SAV	\ Content of VDP(3) register (R#3)
F3E3 const SYS-RG4SAV	\ Content of VDP(4) register (R#4)
F3E4 const SYS-RG5SAV	\ Content of VDP(5) register (R#5)
F3E5 const SYS-RG6SAV	\ Content of VDP(6) register (R#6)
F3E6 const SYS-RG7SAV	\ Content of VDP(7) register (R#7)
F3E7 const SYS-STATFL	\ Content of VDP(8) status register (S#0)
F3E8 const SYS-TRGFLG	\ Information about trigger buttons and space bar state
F3E9 const SYS-FORCLR	\ Foreground color
F3EA const SYS-BAKCLR	\ Background color
F3EB const SYS-BDRCLR	\ Border color
F3EC const SYS-MAXUPD	\ Jump instruction used by Basic LINE command. The routines used are: RIGHTC, LEFTC, UPC and DOWNC
F3EF const SYS-MINUPD	\ Jump instruction used by Basic LINE command. The routines used are: RIGHTC, LEFTC, UPC and DOWNC
F3F2 const SYS-ATRBYT	\ Attribute byte (for graphical routines it’s used to read the color)
F3F3 const SYS-QUEUES	\ Address of the queue table
F3F5 const SYS-FRCNEW	\ CLOAD flag =0 when CLOAD =255 when CLOAD?
F3F6 const SYS-SCNCNT	\ Key scan timing
F3F7 const SYS-REPCNT	\ This is the key repeat delay counter
F3F8 const SYS-PUTPNT	\ Address in the keyboard buffer where a character will be written
F3FA const SYS-GETPNT	\ Address in the keyboard buffer where the next character is read
F3FC const SYS-CS120	\ Cassette I/O parameters to use for 1200 baud
F401 const SYS-CS240	\ Cassette I/O parameters to use for 2400 baud
F406 const SYS-LOW	\ Signal delay when writing a 0 to tape
F408 const SYS-HIGH	\ Signal delay when writing a 1 to tape
F40A const SYS-HEADER	\ Delay of tape header (sync.) block
F40B const SYS-ASPCT1	\ Horizontal / Vertical aspect for CIRCLE command
F40D const SYS-ASPCT2	\ Horizontal / Vertical aspect for CIRCLE command
F40F const SYS-ENDPRG	\ Pointer for the RESUME NEXT command
F414 const SYS-ERRFLG	\ Basic Error code
F415 const SYS-LPTPOS	\ Position of the printer head
F416 const SYS-PRTFLG	\ Printer output flag is read by OUTDO
F417 const SYS-NTMSXP	\ Printer type is read by OUTDO. SCREEN ,,,n writes to this address
F418 const SYS-RAWPRT	\ Raw printer output is read by OUTDO
F419 const SYS-VLZADR	\ Address of data that is temporarilly replaced by ‘O’ when Basic function VAL("") is running
F41B const SYS-VLZDAT	\ Original value that was in the address pointed to with VLZADR
F41C const SYS-CURLIN	\ Line number the Basic interpreter is working on, in direct mode it will be filled with #FFFF
FAF8 const SYS-EXBRSA	\ Slot address of the SUBROM (EXtended Bios-Rom Slot Address)
FB21 const SYS-DRVINF	\ Nr. of drives connected to disk interface 1
FCC1 const SYS-EXPTBL	\ Slot 0: #80 = expanded, 0 = not expanded. Also slot address of the main BIOS-ROM.
FCC5 const SYS-SLTTBL	\ Mirror of slot 0 secondary slot selection register.

: disable-screen ( -- )
    0 0 0 0 0041 SYSCALL 4drop ;

: enable-screen ( -- )
    0 0 0 0 0044 SYSCALL 4drop ;

: vdp! ( data register -- )
    8 lshift + >r
    0 r> 0 0 0047 SYSCALL 4drop ;

: vram@ ( address -- data )
    >r 0 0 0 r> 004a SYSCALL 3drop ;

: vram! ( data address -- )
    >r 0 0 r> 004d SYSCALL 4drop ;

: fill-vram ( data length address -- )
    0 swap 0056 SYSCALL 4drop ;

: vram-to-ram ( length destination source -- )
    >r >r >r 0 r> r> r> 0059 SYSCALL 4drop ;

: ram-to-vram ( length destination source -- )
    >r >r >r 0 r> r> r> 005c SYSCALL 4drop ;

: clear-sprites ( -- )
    0 0 0 0 0069 SYSCALL 4drop ;

: cursor-position ( column row -- )
    8 lshift + >r
    0 0 0 r> 00c6 SYSCALL 4drop ;

: change-color ( bordercolor backgroundcolor foregroundcolor -- )
    sys-forclr c! sys-bakclr c! sys-bdrclr c!
    0 0 0 0 0062 SYSCALL 4drop ;

: init-text ( -- )
    0 0 0 0 006c SYSCALL 4drop ;

: init-mode32 ( -- )
    0 0 0 0 006f SYSCALL 4drop ;

: init-graph ( -- )
    0 0 0 0 0072 SYSCALL 4drop ;

: init-multi ( -- )
    0 0 0 0 0075 SYSCALL 4drop ;

: sprite-pattern ( spriteid -- address )
    0 0 0 0084 SYSCALL nip nip nip ;

: sprite-attribute ( spriteid -- address )
    0 0 0 0087 SYSCALL nip nip nip ;

: sprite-size ( -- size )
    0 0 0 0 008a SYSCALL 3drop ;

decimal
