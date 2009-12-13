SJASM=`which sjasm` -j
CC=gcc -g -std=c99
EMULATOR=openmsx -cart

all: samc test.rom test2.rom

samc: samc.c
	$(CC) samc.c -o samc

%.rom: %.fs
	./samc $? > $(@:.rom=.asm)
	$(SJASM) $(@:.rom=.asm) $@

test: test.rom
	$(EMULATOR) test.rom

test2: test2.rom
	$(EMULATOR) test2.rom

clear:
	rm -f test.asm test.lst test.rom test2.asm test2.lst test2.rom samc
