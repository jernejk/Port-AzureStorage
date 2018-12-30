# Azure storage port

## What is Port-Adapter architecture?

Abstracting out platforms in this manner enables TDD without mocks.

For more info refer to [this presentation](https://1drv.ms/p/s!AkGnX6kNdoCBgcVf-FNGOHn-HgB4gA).

## What is it?

Basic implementation of Azure storage port. Blob storage only for now. Table and Queues will be added as and when required.

Contains:
1. Azure adapter
1. InMemory adapter
1. Unified tests

## How to use it?

Ports are by definition application specific. Therefore, clone it and modify to suit your application.

Please feel free to send across pull requests for bug fixes and/or any API that may be generally applicable.
