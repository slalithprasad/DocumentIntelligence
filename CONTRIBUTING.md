# Contributing to Document Intelligence

Thank you for considering contributing to Document Intelligence! We welcome contributions from everyone. Here are some guidelines to help you get started:

## How to Contribute

1. **Clone the repository**: 
    ```bash
    git clone https://github.com/lalith-kaara/DocumentIntelligence.git
    ```
2. **Restore dependencies**:
    ```bash
    dotnet restore
    ```
3. **Add local settings**: Create a `local.settings.json` file in the root of the project with the necessary configurations.
    ```json
    {
      "IsEncrypted": false,
      "Values": {
        "AzureWebJobsStorage": "",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "OpenAIKey": "<key>",
        "OpenAIUri": "<uri>",
        "OpenAIModel": "<model>"
      }
    }
    ```
4. **Create a branch**: 
    ```bash
    git checkout -b your-branch-name
    ```
5. **Make your changes**: Implement your feature or bug fix.
6. **Commit your changes**: 
    ```bash
    git commit -m "Description of your changes"
    ```
7. **Push to your branch**: 
    ```bash
    git push origin your-branch-name
    ```
8. **Create a pull request**: Go to the original repository and click "New Pull Request".

## Code of Conduct

### Our Pledge

In the interest of fostering an open and welcoming environment, we as contributors and maintainers pledge to make participation in our project and our community a harassment-free experience for everyone, regardless of age, body size, disability, ethnicity, gender identity and expression, level of experience, nationality, personal appearance, race, religion, or sexual identity and orientation.

### Our Standards

Examples of behavior that contributes to creating a positive environment include:

- Using welcoming and inclusive language
- Being respectful of differing viewpoints and experiences
- Gracefully accepting constructive criticism
- Focusing on what is best for the community
- Showing empathy towards other community members

Examples of unacceptable behavior by participants include:

- The use of sexualized language or imagery and unwelcome sexual attention or advances
- Trolling, insulting/derogatory comments, and personal or political attacks
- Public or private harassment
- Publishing others’ private information, such as a physical or electronic address, without explicit permission
- Other conduct which could reasonably be considered inappropriate in a professional setting

### Our Responsibilities

Project maintainers are responsible for clarifying the standards of acceptable behavior and are expected to take appropriate and fair corrective action in response to any instances of unacceptable behavior.

Project maintainers have the right and responsibility to remove, edit, or reject comments, commits, code, wiki edits, issues, and other contributions that are not aligned to this Code of Conduct, or to ban temporarily or permanently any contributor for other behaviors that they deem inappropriate, threatening, offensive, or harmful.

### Scope

This Code of Conduct applies both within project spaces and in public spaces when an individual is representing the project or its community. Examples of representing a project or community include using an official project e-mail address, posting via an official social media account, or acting as an appointed representative at an online or offline event. Representation of a project may be further defined and clarified by project maintainers.

## Reporting Issues

If you find a bug or have a feature request, please create an issue in the [issue tracker](https://github.com/lalith-kaara/DocumentIntelligence/issues).

## Style Guide

- Follow the existing code style.
- Write clear and concise commit messages.
- Include comments and documentation where necessary.

Thank you for your contributions!

Lalith
