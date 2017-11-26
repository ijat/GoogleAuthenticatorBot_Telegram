import pyotp
import sys

try:
    if sys.argv[1] == "totp":
        totp = pyotp.TOTP(sys.argv[2])
        z = totp.now()
    elif sys.argv[1] == "hotp":
        hotp = pyotp.HOTP(sys.argv[2])
        z = hotp.at(sys.argv[3])
    print(z)
    sys.exit(0)
except Exception:
    print("")
    sys.exit(1)