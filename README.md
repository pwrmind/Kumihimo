# Kuhimi Encryption

This project implements a simple encryption and decryption system for strings using a bit-swapping scheme based on a user-provided key. The method is inspired by the Kuhimi technique, which focuses on manipulating bits to achieve encryption.

## Description

The program prompts the user for a string and a key, generates a random bit-swapping scheme based on the key, encrypts the input string, and then decrypts it to demonstrate that the original data can be restored.

## Functionality

- Input a string for encryption.
- Input a key to generate the swapping scheme.
- Encrypt the string using the bit-swapping scheme.
- Decrypt the encrypted string.
- Verify the identity of the swapping scheme upon regeneration.

## Usage

1. Clone the repository to your local machine:
   ```bash
   git clone https://github.com/your_username/your_repository.git
   ```

2. Open the project in your development environment (e.g., Visual Studio).

3. Run the program.

4. Enter the string you want to encrypt and the key for generating the swapping scheme.

5. The program will output the encrypted string and the decrypted string to confirm they match.

## Example

```
Enter a string to encrypt: Hello, World!
Enter a key: mysecretkey
Encrypted string: 48-65-6C-6C-6F-2C-20-57-6F-72-6C-64-21
Decrypted string: Hello, World!
```

## Technologies

- C#
- .NET Framework

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contribution

If you would like to contribute to the project, please fork the repository, make your changes, and create a pull request.
